using UnityEngine;
using UnityEngine.InputSystem;

public class CheatsForTesting : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private int _addCoinAmount;
    [SerializeField]
    private int _removeCoinAmount;


    [Header("References")]
    [SerializeField]
    private IntVariable _playerCoinAmount;
    #endregion

    #region Input Read
    public void OnAddCoins(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCoinAmount.ApplyChange(_addCoinAmount);
        }
    }

    public void OnRemoveCoins(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int targetAmount = _playerCoinAmount.Value - _removeCoinAmount;
            if (targetAmount < 0)
                targetAmount = 0;
            _playerCoinAmount.SetValue(targetAmount);
        }
    }
    #endregion
}
