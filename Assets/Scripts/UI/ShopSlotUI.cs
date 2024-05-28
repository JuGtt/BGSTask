using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemName;
    [SerializeField]
    private TextMeshProUGUI _itemValue;
    #endregion

    #region Private Fields
    private int _id;
    private int _value;
    [SerializeField]
    private ItemSO _item;
    #endregion

    #region Public Fields
    public void SetData(int id)
    {
        _item = GameAssets.Database.GetItemById(id);

        _id = id;
        _value = _item.Value;
        _itemImage.sprite = _item.ItemImage;
        _itemName.SetText(_item.name);
        _itemValue.SetText(_value.ToString());
    }

    public void TryToBuy()
    {
        if (_value < GameAssets.PlayerInventory.CoinAmount)
        {
            BuyItem();
        } else 
            Debug.Log("Not enough coins!");
    }
    #endregion

    #region Private Fields
    private void BuyItem()
    {
        Debug.Log("Buy Item");
        bool bought = GameAssets.PlayerInventory.AddItem(_item, 1);
        if (bought) GameAssets.PlayerInventory.AddCoins(-_value);
    }
    #endregion
}
