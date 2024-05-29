
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private ShopUI _shopUI;
    [SerializeField]
    private GameObject _interactionHightlight;
    #endregion

    #region Public Methods
    public void OpenShop()
    {
        if (_shopUI != null)
            _shopUI.Toggle(true);
    }

    public void CloseShop()
    {
        if (_shopUI != null)
            _shopUI.Toggle(false);
    }

    public void ToggleCanInteract(bool canInteract)
    {
        _interactionHightlight.SetActive(canInteract);
    }
    #endregion
}
