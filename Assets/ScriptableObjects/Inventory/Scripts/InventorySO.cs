using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory")]
public class InventorySO : ScriptableObject
{
    #region Serialized Fields
    [SerializeField]
    private Inventory _inventory;
    #endregion

    #region Private Fields
    private int _coinAmount;
    #endregion

    #region Properties
    public virtual int CoinAmount => _coinAmount;
    public Inventory Inventory => _inventory;
    #endregion

    #region Public Methods
    public void RemoveItemByIndex(int index)
    {
        if (_inventory.Items.Count > index)
            _inventory.Items.RemoveAt(index);
        else
            Debug.Log("Index wasn't found.");
    }

    public void RemoveItemByItemID(int id, bool firstOcurrence)
    {
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            if (_inventory.Items[i].Item.ID == id)
            {
                InventorySlot slot = new InventorySlot();
                _inventory.Items[i] = slot;
                if (firstOcurrence) return;
            }
        }
    }

    public void RemoveItem(int index)
    {
        InventorySlot emptySlot = new InventorySlot(null, 0);
        _inventory.Items[index] = emptySlot;
    }

    public virtual void AddCoins(int amount)
    {
        _coinAmount += amount;
    }

    public bool AddItem(ItemSO newItem, int amount)
    {
        int index = -1;
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            InventorySlot slot = _inventory.Items[i];
            if (!slot.IsEmpty)
            {
                if (newItem != null)
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
            _inventory.Items[index] = newSlot;
            return true;
        }

        Debug.Log("No inventory space.");
        return false;
    }
    #endregion
}

[System.Serializable]
public class Inventory
{
    #region Public Fields
    public List<InventorySlot> Items = new List<InventorySlot>();
    #endregion
}

