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
        InitializeShopUI();
    }
    public void InitializeShopUI()
    {
        if (_uiItems.Count == 0)
        {
            _uiItems.Clear();
            for (int i = 0; i < _inventory.Inventory.Items.Count; i++)
            {
                InventorySlot itemSlot = _inventory.Inventory.Items[i];
                ShopSlotUI shopSlotUI = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _content);
                shopSlotUI.SetData(itemSlot.Item.ID);
                _uiItems.Add(shopSlotUI);
            }
        }
    }
    #endregion
}
