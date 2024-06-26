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
    public void SetData(int id, float fee)
    {
        _item = GameAssets.Database.GetItemById(id);

        _id = id;
        _value = _item.Value + (int)(_item.Value * fee);
        _itemImage.sprite = _item.ItemImage;
        _itemName.SetText(_item.name);
        _itemValue.SetText(_value.ToString());
    }

    public void TryToBuy()
    {
        if (_value < GameAssets.PlayerInventory.CoinAmount)
        {
            BuyItem();
        }
        else
        {
            AudioManager.Instance.PlaySound("Denied", 0.3f);
        }
    }
    #endregion

    #region Private Fields
    private void BuyItem()
    {
        bool bought = GameAssets.PlayerInventory.AddItem(_item, 1);
        if (bought)
        {
            AudioManager.Instance.PlaySound("ItemBuy", 0.2f);
            GameAssets.PlayerInventory.AddCoins(-_value);
        }
    }
    #endregion
}
