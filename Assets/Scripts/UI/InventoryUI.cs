using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField]
    private InventorySlotUI _itemPrefab;
    [SerializeField]
    private RectTransform _contentPanel;
    [SerializeField]
    private GameObject _equipmentTab;
    [SerializeField]
    private TextMeshProUGUI _coinPouch;
    [SerializeField]
    private RectTransform _inventory;
    [SerializeField]
    private EquipmentSlotUI _clothesEquipmentSlot, _hatEquipmentSlot, _hairEquipmentSlot, _underwearEquipmentSlot;
    #endregion

    #region Private Fields
    private List<InventorySlotUI> _uiItems = new List<InventorySlotUI>();
    private int _currentlySelectedItemIndex = -1;
    private PlayerInventorySO _playerInventory;
    #endregion

    #region Public Methods
    public void InitializeInventorySlots()
    {
        _uiItems = new List<InventorySlotUI>();

        int count = _playerInventory.Inventory.Items.Count;
        InventorySlot emptySlot = new InventorySlot();

        // Initialize Empty Slots
        for (int i = 0; i < count; i++)
        {
            InventorySlotUI uiSlot = InitializeUISlot(emptySlot, i);
            _uiItems.Add(uiSlot);
        }

        // Update Coin Value.
        _coinPouch.SetText(_playerInventory.CoinAmount.ToString());

        // Populate Slots with player's inventory.
        UpdateInventoryDisplay();
    }

    public void ToggleInventory(bool show)
    {
        ResetSelectedItem();
        _inventory.gameObject.SetActive(show);
        if (_equipmentTab != null) _equipmentTab.SetActive(show);
        UpdateInventoryDisplay();
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
            UpdateInventoryDisplay();
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
        InitializeInventorySlots();
    }

    private void OnEnable()
    {
        _underwearEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _clothesEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _hairEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _hatEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
    }

    private void OnDisable()
    {
        _underwearEquipmentSlot.OnEquipmentSlotChanged -= HandleEquipmentChange;
        _clothesEquipmentSlot.OnEquipmentSlotChanged -= HandleEquipmentChange;
        _hairEquipmentSlot.OnEquipmentSlotChanged -= HandleEquipmentChange;
        _hatEquipmentSlot.OnEquipmentSlotChanged -= HandleEquipmentChange;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
            UpdateInventoryDisplay();
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
        //TODO: Problem if there two or more of the same item.
        for (int i = 0; i < _uiItems.Count; i++)
        {
            if (_uiItems[i].ID == item.ID)
                return i;
        }
        return -1;
    }

    [ContextMenu("Update Inventory")]
    public void UpdateInventoryDisplay()
    {
        for (int i = 0; i < _playerInventory.Inventory.Items.Count; i++)
        {
            InventorySlot slot = _playerInventory.Inventory.Items[i];
            if (_uiItems.Count <= i)
            {
                _uiItems.Add(InitializeUISlot(new InventorySlot(), i)); // Create Empty
            }
            _uiItems[i].SetData(slot.Item, slot.Amount);
        }
    }

    private void HandleEquipmentChange(ItemSO item)
    {
        Debug.Log("Equip Change");
        ItemSO itemToAdd;
        _playerInventory.RemoveItem(_currentlySelectedItemIndex);

        if (item != null) // Switched gear.
            itemToAdd = item;
        else
            itemToAdd = new InventorySlot(null, 0).Item;

        _playerInventory.AddItem(itemToAdd, 1);
        UpdateInventoryDisplay();
        ResetSelectedItem();
    }

    private void ResetSelectedItem()
    {
        if (_currentlySelectedItemIndex != -1)
            HandleDisselect(_currentlySelectedItemIndex);
        GameManager.Instance.MouseSelection.Toggle(false);
        _currentlySelectedItemIndex = -1;
    }

    private void CreateSelectedItem(ItemSO item, int amount)
    {
        GameManager.Instance.MouseSelection.Toggle(true);
        GameManager.Instance.MouseSelection.SetData(item, amount);
    }

    private void HandleClick(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        //TODO: AUDIO SFX
        GameManager.Instance.ItemHover.Toggle(false);

        //TODO: This index should not be counted on.
        int index = slotUI.Index;

        if (_currentlySelectedItemIndex != -1)
        {
            int selectedIndex = _currentlySelectedItemIndex;
            ResetSelectedItem();
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
        InventorySlot targetSlot = _playerInventory.Inventory.Items[targetIndex];
        InventorySlot selectedSlot = _playerInventory.Inventory.Items[selectedIndex];

        // Handle Stackable Items
        if (targetSlot.Item != null && selectedSlot.Item != null)
        {
            if (targetSlot.Item.ID == selectedSlot.Item.ID && targetSlot.Item.IsStackable)
            {
                int totalAmount = targetSlot.Amount + selectedSlot.Amount;

                if (totalAmount > targetSlot.Item.MaxStackSize)
                {
                    //TODO: Handle swapping amounts if one of the items is at the maximum already
                    int excessAmount = totalAmount - targetSlot.Item.MaxStackSize;
                    targetSlot.Amount = targetSlot.Item.MaxStackSize;
                    selectedSlot.Amount = excessAmount;
                }
                else
                {
                    targetSlot.Amount = totalAmount;
                    selectedSlot = new InventorySlot(); // Clear the selected slot
                }
                _playerInventory.Inventory.Items[targetIndex] = targetSlot;
                _playerInventory.Inventory.Items[selectedIndex] = selectedSlot;

                //TODO: AUDIO SFX;
                UpdateInventoryDisplay();
                return;
            }
        }
        _playerInventory.Inventory.Items[targetIndex] = selectedSlot;
        _playerInventory.Inventory.Items[selectedIndex] = targetSlot;

        //TODO: AUDIO SFX;

        //Update UI
        UpdateInventoryDisplay();
    }

    private void HandleEnter(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        if (_currentlySelectedItemIndex != -1)
            if (_uiItems[_currentlySelectedItemIndex] == slotUI)
                return; // Shouldn't show Hover if you're targetting the selected item.

        //TODO: Audio SFX.

        GameManager.Instance.ItemHover.Toggle(true);
        GameManager.Instance.ItemHover.UpdateHover(item);
    }

    private void HandleExit()
    {
        GameManager.Instance.ItemHover.Toggle(false);
    }
    #endregion
}
