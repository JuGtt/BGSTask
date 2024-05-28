using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private bool _canMove = true;
    [Header("References")]
    [SerializeField]
    private InventorySlotUI _itemPrefab;
    [SerializeField]
    private RectTransform _contentPanel;
    [SerializeField]
    private MouseSelection _mouseFollower;
    [SerializeField]
    private ItemHover _itemHover;
    [SerializeField]
    private GameObject _equipmentTab;
    [SerializeField]
    private TextMeshProUGUI _coinPouch;
    [SerializeField]
    private RectTransform _inventory;
    [SerializeField]
    private EquipmentSlotUI _clothesEquipmentSlot;
    #endregion

    #region Private Fields
    private List<InventorySlotUI> _uiItems = new List<InventorySlotUI>();
    private int _currentlySelectedItemIndex = -1;
    private PlayerInventorySO _playerInventory;
    #endregion

    #region Public Methods
    public void InitializeInventoryUI()
    {
        _uiItems = new List<InventorySlotUI>();

        Inventory thisInventory = _playerInventory.Inventory;
        List<InventorySlot> items = thisInventory.Items;
        int count = items.Count;

        // Initialize Slots
        for (int i = 0; i < count; i++)
        {
            InventorySlotUI uiSlot = InitializeUISlot(items[i], i);
            _uiItems.Add(uiSlot);
        }

        _coinPouch.SetText(_playerInventory.CoinAmount.ToString());
    }

    public void ToggleInventory(bool show)
    {
        ResetSelectedItem();
        _inventory.gameObject.SetActive(show);
        if (_equipmentTab != null) _equipmentTab.SetActive(show);
        UpdateInventory();
    }

    public bool AddItem(ItemSO newItem, int amount)
    {
        int index = -1;
        for (int i = 0; i < _playerInventory.Inventory.Items.Count; i++)
        {
            InventorySlot slot = _playerInventory.Inventory.Items[i];
            if (!slot.IsEmpty)
            {
                if (slot.Item.ID == newItem.ID) // Same Item
                    if (slot.Item.IsStackable) // Stackable
                    {
                        amount = slot.Amount + amount;
                        index = i;
                        break;
                    }
            }
            else
            {
                // Open Slot
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            Debug.Log("Added item at index: " + index);
            InventorySlot newSlot = new InventorySlot(newItem, amount);
            _playerInventory.Inventory.Items[index] = newSlot;
            UpdateInventory();
            return true;
        }

        Debug.Log("No inventory space.");
        return false;
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        _playerInventory = GameAssets.PlayerInventory;
        InitializeInventoryUI();
    }

    private void OnEnable()
    {
        _clothesEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
    }

    private void OnDisable()
    {
        _clothesEquipmentSlot.OnEquipmentSlotChanged -= HandleEquipmentChange;
    }

    private InventorySlotUI InitializeUISlot(InventorySlot slot, int index)
    {
        bool isSlotEmpty = slot.IsEmpty;

        InventorySlotUI uiSlot = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
        uiSlot.Index = index; // TODO:
        uiSlot.name = "InventoryUISlot";
        uiSlot.transform.SetParent(_contentPanel);
        uiSlot.transform.localScale = Vector3.one;

        uiSlot.OnSlotClicked += HandleClick; // Selection
        uiSlot.OnSlotEnter += HandleEnter; // Hover Enter
        uiSlot.OnSlotExit += HandleExit; // Hover Exit

        uiSlot.SetData(slot.Item, slot.Amount);

        return uiSlot;
    }

    private int GetIndexOfInventoryItem(ItemSO item)
    {
        for (int i = 0; i < _uiItems.Count; i++)
        {
            if (_uiItems[i].ID == item.ID)
                return i;
        }
        return -1;
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < _uiItems.Count; i++)
        {
            InventorySlot slot = _playerInventory.Inventory.Items[i];
            _uiItems[i].SetData(slot.Item, slot.Amount);
        }
    }

    private void HandleEquipmentChange(ItemSO item)
    {
        ItemSO itemToAdd;
        _playerInventory.RemoveItem(_currentlySelectedItemIndex);

        if (item != null) // Switched gear.
            itemToAdd = item;
        else
            itemToAdd = new InventorySlot(null, 0).Item;

        _playerInventory.AddItem(itemToAdd, 1);
        UpdateInventory();
        ResetSelectedItem();
    }

    private void ResetSelectedItem()
    {
        if (_currentlySelectedItemIndex != -1)
            HandleDisselect(_currentlySelectedItemIndex);
        _mouseFollower.Toggle(false);
        _currentlySelectedItemIndex = -1;
    }

    private void CreateSelectedItem(ItemSO item, int amount)
    {
        _mouseFollower.Toggle(true);
        _mouseFollower.SetData(item, amount);
    }

    private void HandleClick(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        _itemHover.Toggle(false);

        //TODO: This index should not be counted on.
        int index = slotUI.Index;

        if (_currentlySelectedItemIndex != -1)
        {
            ResetSelectedItem();
            int selectedIndex = GetIndexOfInventoryItem(_mouseFollower.SelectedItem);
            HandleDisselect(selectedIndex);
            HandleSwap(selectedIndex, index);
            return;
        }
        if (slotUI.IsEmpty)
            return;

        HandleSelect(item, amount, index);
        _currentlySelectedItemIndex = index;
    }

    private void HandleSelect(ItemSO item, int amount, int index)
    {
        _uiItems[index].Select();
        CreateSelectedItem(item, amount);
    }

    private void HandleDisselect(int index)
    {
        _uiItems[index].Disselect();
    }

    private void HandleSwap(int selectedIndex, int targetIndex)
    {
        if (targetIndex == -1)
        {
            Debug.Log("Index was not found.");
            return;
        }

        //Handle Inventory Change
        InventorySlot targetSlot = _playerInventory.Inventory.Items[targetIndex];
        _playerInventory.Inventory.Items[targetIndex] = _playerInventory.Inventory.Items[selectedIndex];
        _playerInventory.Inventory.Items[selectedIndex] = targetSlot;

        //Update UI
        UpdateInventory();
    }

    private void HandleEnter(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        if (_currentlySelectedItemIndex != -1)
            if (_uiItems[_currentlySelectedItemIndex] == slotUI)
                return;

        _itemHover.Toggle(true);
        _itemHover.UpdateHover(item);
    }

    private void HandleExit()
    {
        _itemHover.Toggle(false);
    }
    #endregion
}
