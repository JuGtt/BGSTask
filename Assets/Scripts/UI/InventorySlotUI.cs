using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Events
    public event Action<InventorySlotUI, ItemSO, int> OnSlotClicked, OnSlotEnter;
    public event Action OnSlotExit;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private Image _borderImage;
    [SerializeField]
    private TextMeshProUGUI _quantityText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private bool _sell;
    #endregion

    #region Private Fields
    private int _id;
    private int _quantity;
    private bool _empty = true;
    private ItemDataBaseObject _database;
    private ItemSO _item;
    private int _index;
    #endregion

    #region Properties
    public int ID => _id;
    public int Quantity => _quantity;
    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }
    public bool IsEmpty => _empty;
    public bool Sell
    {
        get { return _sell; }
        set { _sell = value; }
    }
    
    public Sprite ItemImage => _itemImage.sprite;
    #endregion

    #region Public Methods
    public void TryToSell()
    {
        if (_empty)
            return;

        PlayerInventorySO playerInventory = GameAssets.PlayerInventory;
        ItemSO item = _database.GetItemById(_id);
        //playerInventory.RemoveItemByIndex();
        playerInventory.AddCoins(item.Value * _quantity);
    }

    public void ResetData()
    {
        if (_button != null)
            _button.interactable = false;

        _item = null;
        _itemImage.gameObject.SetActive(false);
        _id = -1;
        _empty = true;
    }

    public void Select()
    {
        _itemImage.enabled = false;
        _quantityText.SetText("");
    }

    public void Disselect()
    {
        _itemImage.enabled = true;
        SetQuantityText();
    }

    public void SetData(ItemSO item, int amount)
    {
        _item = item;
        if (item == null)
        {
            ResetData();
            return;
        }

        if (_button != null)
            _button.interactable = true;

        _itemImage.gameObject.SetActive(true);
        _id = _item.ID;
        _itemImage.sprite = _item.ItemImage;
        _quantity = amount;

        SetQuantityText();

        _empty = false;
    }

    #region Actions
    public void OnPointerClick(PointerEventData pointerData)
    {
        OnSlotClicked?.Invoke(this, _item, _quantity);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_empty)
            return;

        OnSlotEnter?.Invoke(this, _item, _quantity);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSlotExit?.Invoke();
    }
    #endregion

    #endregion

    #region Private Methods
    private void Awake()
    {
        _database = GameAssets.Database;
        ResetData();
    }
    private void SetQuantityText()
    {
        if (_quantity == 1)
            _quantityText.SetText("");
        else
            _quantityText.SetText(_quantity.ToString());
    }
    #endregion
}
