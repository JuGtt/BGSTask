[System.Serializable]
public class InventorySlot
{
    #region Private Fields
    public ItemSO Item;
    public int Amount;
    #endregion

    #region Properties
    public bool IsEmpty => Amount == 0;
    #endregion

    #region Constructor
    public InventorySlot()
    {
        Item = null;
        Amount = 0;
    }
    public InventorySlot(ItemSO item, int amount)
    {
        Item = item;
        Amount = amount;
    }
    #endregion

    #region Public Methods
    public void AddAmount(int value)
    {
        Amount += value;
    }
    #endregion
}