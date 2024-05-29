using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHover : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private TextMeshProUGUI _itemName, _itemType, _itemDescription, _itemSellValue;
    [SerializeField]
    private float _followSpeed = 15f;
    [SerializeField]
    private Vector2 _offset;
    #endregion

    #region Private Fields
    private Vector2 _mousePos;
    #endregion

    #region Input Read
    public void OnMousePosition(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }
    #endregion

    #region Public Methods
    public void Toggle(bool show)
    {
        gameObject.SetActive(show);
    }
    public void UpdateHover(ItemSO item)
    {
        if (item == null)
        {
            // If hovered over Empty Inventory Slot.
            Toggle(false);
            return;
        }

        _itemName.SetText(item.name);
        if (item.ItemType != null)
            _itemType.SetText(item.ItemType.name.ToString());
        else
            _itemType.SetText("");

        _itemDescription.SetText(item.Description);
        _itemSellValue.SetText(item.Value.ToString());

        transform.position = _mousePos + _offset;
    }
    #endregion

    #region Private Methods
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _mousePos + _offset, _followSpeed * Time.deltaTime);
    }
    #endregion
}
