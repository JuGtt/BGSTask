using UnityEngine;

[CreateAssetMenu(fileName = "New Game Assets")]
public class GameAssets : ScriptableObject
{
    #region Static Fields
    private static GameAssets s_instance;
    #endregion

    #region Serialized Fields
    [SerializeField]
    private ItemDataBaseObject _globalDatabase;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    #endregion

    #region Properties
    public static ItemDataBaseObject Database => s_instance._globalDatabase;
    public static PlayerInventorySO PlayerInventory => s_instance._playerInventory;
    #endregion

    #region Initialize Instance
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        s_instance = Resources.Load<GameAssets>("Singletons/GameAssets");
    }
    #endregion
}
