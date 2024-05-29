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
    [SerializeField, Tooltip("By how much the screen will divide to find the optimal offset.")]
    private float _divider = 1f;
    #endregion

    #region Private Fields
    private Vector2 _mousePos;
    private Vector2 _offset;
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
    private void Start()
    {
        SetOffSetByScreenSize();
    }
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _mousePos + _offset, _followSpeed * Time.deltaTime);
    }
    private void SetOffSetByScreenSize()
    {
        Vector2 newOffset = new Vector2(Screen.width / _divider, -Screen.height / _divider);
        _offset = newOffset;
    }
    #endregion
}
