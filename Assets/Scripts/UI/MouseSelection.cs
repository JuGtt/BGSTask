
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSelection : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private InventorySlotUI _inventorySlotUI;
    #endregion

    #region Private Fields
    private Canvas _canvas;
    private Vector2 _mousePos;
    [SerializeField]
    private ItemSO _selectedItem;
    #endregion

    #region Properties
    public ItemSO SelectedItem => _selectedItem;
    #endregion

    #region Input Read
    public void OnMousePosition(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }
    #endregion

    #region Public Methods
    public void SetData(ItemSO item, int quantity)
    {
        if (item == null)
        {
            _inventorySlotUI.ResetData();
            return;
        }
        _selectedItem = item;
        _inventorySlotUI.SetData(item, quantity);
    }

    public void Toggle(bool show)
    {
        gameObject.SetActive(show);
        if (!show)
            SetData(null, 0);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, _mousePos, _canvas.worldCamera, out position);
        transform.position = _canvas.transform.TransformPoint(position);
    }
    #endregion
}
