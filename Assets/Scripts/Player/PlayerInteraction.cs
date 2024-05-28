using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField, Tooltip("Which layers are interactable?")]
    private LayerMask _layerMask;    
    #endregion

    #region Private Fields
    private Vector2 _mousePos;
    #endregion

    #region Input Read
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && CanInteract())
        {
            TryToInteract();
        }
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }
    #endregion

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    private void TryToInteract()
    {
    }
    #endregion

    #region Action Checks
    private bool CanInteract()
    {
        return true;
    }
    #endregion
}
