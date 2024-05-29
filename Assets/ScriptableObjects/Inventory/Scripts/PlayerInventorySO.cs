using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory")]
public class PlayerInventorySO : InventorySO
{
    #region Serialized Fields
    [SerializeField]
    private IntVariable _playerCoinAmount;
    #endregion

    #region Properties
    public override int CoinAmount => _playerCoinAmount.Value;
    #endregion

    #region Public Methods
    public override void AddCoins(int amount)
    {
        if (_playerCoinAmount + amount < 0)
        {
            AudioManager.Instance.PlaySound("Denied", 0.1f);
            Debug.Log("Not enough coins.");
            return;
        }
        _playerCoinAmount.ApplyChange(amount);
    }
    #endregion
}
