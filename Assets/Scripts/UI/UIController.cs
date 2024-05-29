using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private int _npcLayer = 3;
    [SerializeField]
    private PlayerInput _playerInputManager;
    [SerializeField]
    private InventoryUI _inventoryUI;

    [Header("References")]
    private bool _onUI;
    #endregion

    #region Private Fields
    private Shopkeeper _shopkeeper;
    #endregion;

    #region Input Read
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onUI = !_onUI;
            if (_shopkeeper != null)
            {
                if (_onUI)
                    _shopkeeper.OpenShop();
                else
                    _shopkeeper.CloseShop();
            }
            else
            {
                _inventoryUI.ToggleInventory(_onUI);
            }
        }
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        _onUI = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == _npcLayer)
        {
            _shopkeeper = other.transform.root.GetComponent<Shopkeeper>();
            _shopkeeper.ToggleCanInteract(true);
            _inventoryUI.ToggleInventory(false);
            _onUI = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == _npcLayer)
        {
            if (_shopkeeper != null)
            {
                _shopkeeper.ToggleCanInteract(false);
                _shopkeeper.CloseShop();
                _shopkeeper = null;
                _onUI = false;
            }
        }
    }
    #endregion
}
