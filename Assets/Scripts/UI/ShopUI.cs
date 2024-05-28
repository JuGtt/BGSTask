using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private ShopSlotUI _itemPrefab;
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private InventorySO _inventory;
    #endregion

    #region Private Fields
    private List<ShopSlotUI> _uiItems = new List<ShopSlotUI>();
    #endregion

    #region Public Methods
    public void Toggle(bool show)
    {
        gameObject.SetActive(show);
        //UpdateInventoryDisplay(); // Enable if disabling on update.
    }

    public void InitializeShopUI()
    {
        ClearShop();
        for (int i = 0; i < _inventory.Inventory.Items.Count; i++)
        {
            InventorySlot itemSlot = _inventory.Inventory.Items[i];
            ShopSlotUI shopSlotUI = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _content);
            shopSlotUI.SetData(itemSlot.Item.ID);
            _uiItems.Add(shopSlotUI);
        }
    }

    public void UpdateInventoryDisplay()
    {
        for (int i = 0; i < _inventory.Inventory.Items.Count; i++)
        {
            InventorySlot slot = _inventory.Inventory.Items[i];
            if (_uiItems.Count <= i)
            {
                ShopSlotUI shopSlotUI = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _content);
                _uiItems.Add(shopSlotUI);
            }
            _uiItems[i].SetData(slot.Item.ID);
        }
    }

    public void ClearShop()
    {
        _uiItems.Clear();
        for (int i = 0; i < _content.childCount; i++)
        {
            GameObject child = _content.GetChild(i).gameObject;
            Destroy(child);
        }
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        InitializeShopUI();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
            UpdateInventoryDisplay();
    }
    #endregion
}
