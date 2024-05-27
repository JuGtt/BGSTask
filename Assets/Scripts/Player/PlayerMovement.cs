using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Public Fields
    public static event Action<Vector2> OnMoveAction;
    #endregion

    #region Serialized Fields
    [Header("Player Settings")]
    [SerializeField]
    private float _moveSpeed = 5f, _runningSpeed = 8f;

    [Header("References")]
    [SerializeField]
    private PlayerAnimationController _playerAnimatorController;
    #endregion

    #region Private Fields    
    // References
    private Rigidbody2D _rb;

    // Input
    private bool _isRunningButtonDown;
    private Vector2 _moveInput;

    // Player State
    private bool _isRunning;
    #endregion

    #region Properties
    public bool IsRunning => _isRunning;
    #endregion

    #region Input Read
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        OnMoveAction?.Invoke(_moveInput);
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
