using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    #region Event
    public event Action<ItemSO> OnEquipmentSlotChanged;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private Image _itemImage;
    [SerializeField, Tooltip("Which Item Type can be equipped here.")]
    private ItemType _itemType;
    [SerializeField]
    private ItemSO _equippedItem;
    [SerializeField]
    private MouseSelection _mouseSelection;
    #endregion

    #region Properties
    public ItemSO EquippedItem => _equippedItem;
    #endregion

    #region Public Methods
    [ContextMenu("Update Equipped Item")]
    public void ChangeEquippedItem()
    {
        ItemSO item = _mouseSelection.SelectedItem;
        if (item == null) return;
        if (item.ItemType != _itemType)
        {
            //TODO: Add audio feedback.
            Debug.Log("Cant Equip this here!");
            return;
        }
        ItemSO oldItem = _equippedItem;
        _equippedItem = item;
        _itemImage.sprite = _equippedItem.ItemImage;
        OnEquipmentSlotChanged?.Invoke(oldItem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked.");
    }
    #endregion
}
