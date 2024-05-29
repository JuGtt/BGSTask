using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Serialized Fields
    [Header("Settings")]
    [SerializeField]
    private bool _inShop = false;

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

        // Populate Slots with player's inventory.
        UpdateInventoryDisplay();
    }

    public void ToggleInventory(bool show)
    {
        ResetSelectedItem();
        _inventory.gameObject.SetActive(show);
        if (_equipmentTab != null) _equipmentTab.SetActive(show);
        UpdateInventoryDisplay();
        if (show)
            AudioManager.Instance.PlaySound("UIOpen", 0.3f);
        else
            AudioManager.Instance.PlaySound("UIClose", 0.3f);
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
            InventorySlot newSlot = new InventorySlot(newItem, amount);
            _playerInventory.Inventory.Items[index] = newSlot;
            UpdateInventoryDisplay();
            return true;
        }

        AudioManager.Instance.PlaySound("Denied", 0.3f);
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
        if (_inShop)
            return;
        _underwearEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _clothesEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _hairEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
        _hatEquipmentSlot.OnEquipmentSlotChanged += HandleEquipmentChange;
    }

    private void OnDisable()
    {
        if (_inShop)
            return;
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
        uiSlot.Index = index;
        uiSlot.name = "InventoryUISlot";
        uiSlot.transform.SetParent(_contentPanel);
        uiSlot.transform.localScale = Vector3.one;

        uiSlot.OnSlotClicked += HandleClick; // Selection
        uiSlot.OnSlotEnter += HandleEnter; // Hover Enter
        uiSlot.OnSlotExit += HandleExit; // Hover Exit

        if (_inShop)
            uiSlot.Sell = true;

        uiSlot.SetData(slot.Item, slot.Amount);

        return uiSlot;
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

        if (_uiItems.Count > _playerInventory.Inventory.Items.Count)
        {
            // Remove excess UI items
            for (int i = _uiItems.Count - 1; i >= _playerInventory.Inventory.Items.Count; i--)
            {
                Destroy(_uiItems[i].gameObject); // Optionally, destroy the GameObject if needed
                _uiItems.RemoveAt(i);
            }
        }

        //Update Coint Pouch
        _coinPouch.SetText(_playerInventory.CoinAmount.ToString());
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
        UpdateInventoryDisplay();
        ResetSelectedItem();

        AudioManager.Instance.PlaySound("ItemEquip", 0.3f);
    }

    private void ResetSelectedItem()
    {
        if (_currentlySelectedItemIndex != -1)
            HandleDisselect(_currentlySelectedItemIndex);
        GameManager.Instance.MouseSelection.Toggle(false);
        GameManager.Instance.MouseSelection.SetData();
        _currentlySelectedItemIndex = -1;
    }

    private void CreateSelectedItem(ItemSO item, int amount)
    {
        GameManager.Instance.MouseSelection.Toggle(true);
        GameManager.Instance.MouseSelection.SetData(item, amount);
    }

    private void HandleClick(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        //AudioManager.Instance.PlaySound("Click", 0.2f);
        GameManager.Instance.ItemHover.Toggle(false);

        if (slotUI.Sell)
        {
            HandleSell(slotUI, item, amount);
            return;
        }

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

    private void HandleSell(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        if (slotUI.IsEmpty) return;

        AudioManager.Instance.PlaySound("ItemSell", 0.2f);

        GameAssets.PlayerInventory.Inventory.Items[slotUI.Index] = new InventorySlot();
        GameAssets.PlayerInventory.AddCoins(item.Value);

        UpdateInventoryDisplay();
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
                    //TODO: Maybe handle swapping amounts if one of the items is at the maximum already
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

                UpdateInventoryDisplay();
                return;
            }
        }
        _playerInventory.Inventory.Items[targetIndex] = selectedSlot;
        _playerInventory.Inventory.Items[selectedIndex] = targetSlot;

        //Update UI
        UpdateInventoryDisplay();
    }

    private void HandleEnter(InventorySlotUI slotUI, ItemSO item, int amount)
    {
        AudioManager.Instance.PlaySound("ItemHover", 0.2f);

        if (_currentlySelectedItemIndex != -1)
            if (_uiItems[_currentlySelectedItemIndex] == slotUI)
                return; // Shouldn't show Hover if you're targetting the selected item.

        GameManager.Instance.ItemHover.Toggle(true);
        GameManager.Instance.ItemHover.UpdateHover(item);
    }

    private void HandleExit()
    {
        GameManager.Instance.ItemHover.Toggle(false);
    }
    #endregion
}
