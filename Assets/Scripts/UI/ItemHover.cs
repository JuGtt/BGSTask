using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHover : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private TextMeshProUGUI _itemName, _itemType, _itemDescription;
    [SerializeField]
    private float _followSpeed = 15f;
    [SerializeField]
    private Vector2 _offset;
    #endregion

    #region Private Fields
    private ItemDataBaseObject _database;
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
            Toggle(false);
            return;
        }

        _itemName.SetText(item.name);
        if (item.ItemType != null)
            _itemType.SetText(item.ItemType.ToString());
        else
            _itemType.SetText("");
        _itemDescription.SetText(item.Description);

        transform.position = _mousePos + _offset;
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _database = GameAssets.Database;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _mousePos + _offset, _followSpeed * Time.deltaTime);
    }
    #endregion
}
