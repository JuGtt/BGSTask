using UnityEngine;

public class PulsatingEffect : MonoBehaviour
{
    #region Serialized Fields
    [Header("Settings")]
    [SerializeField]
    private float pulsateSpeed = 1.0f;
    [SerializeField]
    private float maxScale = 1.5f;
    [SerializeField]
    private float minScale = 0.5f;
    #endregion

    #region Private Fields
    private Vector3 originalScale;
    private float pulsateTimer;
    #endregion

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        pulsateTimer += Time.deltaTime * pulsateSpeed;
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(pulsateTimer) + 1.0f) / 2.0f);
        transform.localScale = originalScale * scaleFactor;
    }
}
