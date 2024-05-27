using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [Header("Player Settings")]
    [SerializeField]
    private float _moveSpeed = 5f, _runningSpeed = 8f;

    [Header("References")]
    [SerializeField]
    private Animator[] _animators;

    #endregion

    #region Private Fields    
    // References
    private Rigidbody2D _rb;

    // Input
    private bool _isRunningButtonDown;
    private Vector2 _moveInput;
    private Vector2 _lookDirection;

    // Player State
    private bool _isRunning;
    #endregion

    #region Input Read
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_moveInput.sqrMagnitude > 0.1f)
            _lookDirection = _moveInput;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isRunningButtonDown = true;
                break;
            case InputActionPhase.Canceled:
                _isRunningButtonDown = false;
                break;
        }
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Subscribe Clothes Change to Update Animator
    }

    private void Update()
    {
        UpdateIsRunning();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (CanMove()) Move();
    }

    private void Move()
    {
        float targetSpeed = _isRunning ? _runningSpeed : _moveSpeed;
        _rb.MovePosition(_rb.position + _moveInput * targetSpeed * Time.fixedDeltaTime);
    }

    private void UpdateIsRunning()
    {
        _isRunning = _isRunningButtonDown && CanRun();
    }

    private void UpdateAnimation()
    {
        float speed = _moveInput.sqrMagnitude;
        Vector2 moveInput = _lookDirection;
        if (speed >= 0.1f)
        {
            if (!_isRunning)
            {
                moveInput *= 2;
            }
            else if (_isRunning)
            {
                moveInput *= 3;
            }
        }
        // _animators.SetFloat("Speed", speed);
        // _animators.SetBool("IsRunning", _isRunning);
        // _animators.SetFloat("Horizontal", moveInput.x);
        // _animators.SetFloat("Vertical", moveInput.y);
    }

    private void UpdateAnimators()
    {

    }
    #endregion

    #region Action Checks
    private bool CanMove()
    {
        return true;
    }

    private bool CanRun()
    {
        return true;
    }
    #endregion
}
