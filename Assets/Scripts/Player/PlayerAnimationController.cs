using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField]
    private BoolVariable _isRunning;
    #endregion
    
    #region Private Fields
    private Animator _baseAnimator;
    private Animator[] _clothingAnimators;
    private Vector2 _moveInput;
    private Vector2 _lookDirection;
    #endregion

    #region Public Methods
    public void PopulateClothingAnimators()
    {
        int animatorsCount = transform.childCount;
        _clothingAnimators = new Animator[animatorsCount];

        for (int i = 0; i < animatorsCount; i++)
        {
            _clothingAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
        }
    }

    public void OnMove(Vector2 movement)
    {
        _moveInput = movement;
        if (_moveInput.sqrMagnitude > 0.1f)
            _lookDirection = _moveInput;
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _baseAnimator = GetComponent<Animator>();
        PopulateClothingAnimators();
        _lookDirection = new Vector2(0, -1); // Start Facing Down.
    }

    private void OnEnable()
    {
        PlayerMovement.OnMoveAction += OnMove;
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoveAction -= OnMove;
    }

    private void UpdateAnimation()
    {
        float speed = _moveInput.sqrMagnitude;
        Vector2 moveInput = _moveInput;

        if (speed >= 0.1f)
            moveInput *= _isRunning ? 3 : 2;
        else
            moveInput = _lookDirection;


        // Sync Animation Across All Clothing
        _baseAnimator.SetFloat("Horizontal", moveInput.x);
        _baseAnimator.SetFloat("Vertical", moveInput.y);
        foreach (Animator animator in _clothingAnimators)
        {
            if (animator.runtimeAnimatorController != null)
            {
                animator.SetFloat("Horizontal", moveInput.x);
                animator.SetFloat("Vertical", moveInput.y);
            }
        }
    }
    #endregion
}
