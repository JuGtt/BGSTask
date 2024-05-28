using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayIntVariable : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private string _textAppend;
    [SerializeField]
    private IntVariable _intVariable;
    #endregion

    #region Private Fields
    private TextMeshProUGUI _text;
    #endregion

    #region Private Methods    
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        _text.SetText(_textAppend + _intVariable.Value.ToString());
    }
    #endregion
}
