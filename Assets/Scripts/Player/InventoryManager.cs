using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private InventorySO _inventory;
    #endregion

    #region Properties
    public InventorySO Inventory => _inventory;
    #endregion
}
