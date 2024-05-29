using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    #region Serialized Fields
    [Header("Settings")]
    [SerializeField, Tooltip("The fee rate for the items sell value.")]
    private float _shopFee = 0.05f;

    [Header("References")]
    [SerializeField, Tooltip("What will sell here?")]
    private InventorySO _inventory;
    [SerializeField]
    private ShopSlotUI _shopItemUIPrefab;
    [SerializeField]
    private RectTransform _content;
    #endregion

    #region Private Fields
    private List<ShopSlotUI> _uiItems = new List<ShopSlotUI>();
    #endregion

    #region Public Methods
    public void Toggle(bool show)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(show);
        }

        if (show)
            AudioManager.Instance.PlaySound("ItemBuy", 0.2f);
        //UpdateInventoryDisplay(); // Enable if disabling on update.
    }

    public void InitializeShopUI()
    {
        ClearShop();
        for (int i = 0; i < _inventory.Inventory.Items.Count; i++)
        {
            InventorySlot itemSlot = _inventory.Inventory.Items[i];
            ShopSlotUI shopSlotUI = Instantiate(_shopItemUIPrefab, Vector3.zero, Quaternion.identity, _content);
            shopSlotUI.SetData(itemSlot.Item.ID, _shopFee);
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
                ShopSlotUI shopSlotUI = Instantiate(_shopItemUIPrefab, Vector3.zero, Quaternion.identity, _content);
                _uiItems.Add(shopSlotUI);
            }
            _uiItems[i].SetData(slot.Item.ID, _shopFee);
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
