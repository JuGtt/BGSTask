using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create New Item")]
public class ItemSO : ScriptableObject
{
    #region Serialized Fields
    [SerializeField]
    private int _id;
    [SerializeField]
    private bool _isStackable;
    [SerializeField]
    private bool _canSell = true;
    [SerializeField]
    private int _maxStackSize = 1;
    [SerializeField, TextArea(5, 10)]
    private string _description;
    [SerializeField]
    private ItemType _itemType;
    [SerializeField]
    private Sprite _UIDisplay;
    [SerializeField]
    private AnimatorOverrideController _animator;
    [SerializeField]
    private int _value;
    #endregion

    #region Properties
    public AnimatorOverrideController Animator => _animator;
    public bool IsStackable => _isStackable;
    public bool CanSell => _canSell;
    public int MaxStackSize => _maxStackSize;
    public string Description => _description;
    public ItemType ItemType => _itemType;
    public Sprite ItemImage => _UIDisplay;
    public int Value => _value;
    public int ID
    {
        get { return _id; }
        set { _id = value;}
    }
    #endregion

}

[System.Serializable]
public class Item
{
    #region Public Fields
    public string Name;
    public int ID;
    public bool IsStackable { get; private set; }
    public Sprite ItemImage { get; private set; }
    public ItemType ItemType { get; private set; }
    public string ItemDescription { get; private set; }
    #endregion

    #region Constructor
    public Item(ItemSO item)
    {
        Name = item.name;
        ID = item.ID;
        IsStackable = item.IsStackable;
        ItemImage = item.ItemImage;
        ItemType = item.ItemType;
        ItemDescription = item.Description;
    }
    #endregion
}