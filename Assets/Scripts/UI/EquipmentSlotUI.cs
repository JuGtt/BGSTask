using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    #region Static Events
    public static event Action OnEquipmentChanged;
    #endregion

    #region Public Events
    public event Action<ItemSO> OnEquipmentSlotChanged;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private Image _itemImage;
    [SerializeField, Tooltip("Which Item Type can be equipped here.")]
    private ItemType _itemType;
    [SerializeField]
    private ItemSO _equippedItem;
    #endregion

    #region Properties
    public ItemSO EquippedItem => _equippedItem;
    #endregion

    #region Public Methods
    public void ChangeEquippedItem(ItemSO newItem)
    {
        ItemSO item = newItem;
        if (item == null)
            return;

        if (item.ItemType != _itemType)
        {
            AudioManager.Instance.PlaySound("Denied", 0.3f);
            Debug.Log("Cant Equip this here!");
            return;
        }
        _itemImage.enabled = true;
        ItemSO oldItem = _equippedItem;
        _equippedItem = item;
        _itemImage.sprite = _equippedItem.ItemImage;
        GameManager.Instance.MouseSelection.SetData();
        OnEquipmentSlotChanged?.Invoke(oldItem);
        OnEquipmentChanged?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemSO item = GameManager.Instance.MouseSelection.SelectedItem;
        if (item != null) // Swapping
            ChangeEquippedItem(item);
        else // Unequiping        
            Unequip();
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        UpdateEquippedItem();
    }

    private void UpdateEquippedItem()
    {
        if (_equippedItem == null)
        {
            _itemImage.enabled = false;
        }
    }

    private void Unequip()
    {
        bool unequiped = GameAssets.PlayerInventory.AddItem(_equippedItem, 1);
        if (unequiped)
        {
            _equippedItem = null;
            UpdateEquippedItem();
            OnEquipmentChanged?.Invoke();
        }
        else
        {
            AudioManager.Instance.PlaySound("Denied", 0.3f);
        }
    }
    #endregion
}
