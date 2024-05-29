
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private ShopUI _shopUI;
    [SerializeField]
    private GameObject _interactionHighlight;
    #endregion

    #region Public Methods
    public void OpenShop()
    {
        _shopUI.Toggle(true);
    }

    public void CloseShop()
    {
        _shopUI.Toggle(false);
    }
    #endregion

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _interactionHighlight.SetActive(true);
            OpenShop();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _interactionHighlight.SetActive(false);
            CloseShop();
        }
    }
    #endregion
}
