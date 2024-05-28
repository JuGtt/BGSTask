using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    #region Static Events
    public static event Action OnToggleMenu;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private PlayerInput _playerInputManager;
    [SerializeField]
    private InventoryUI _inventoryUI;
    #endregion

    #region Private Fields
    private bool _onUI;
    #endregion;

    #region Input Read
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onUI = !_onUI;
            _inventoryUI.ToggleInventory(_onUI);

            if (_onUI)            
                _playerInputManager.SwitchCurrentActionMap("UI");            
            else
                _playerInputManager.SwitchCurrentActionMap("Player");            
        }
    }
    #endregion
}
