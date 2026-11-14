using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public UnityAction OnJump;
    public UnityAction OnHitGround;

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundForce = -2f;

    private float _verticalVelocity;

    [Header("Coyote Time")] // A small safe time to jump after is ungrounded
    [SerializeField] private float _coyoteTime = 0.15f;

    private float _coyoteTimeCounter;
    private bool _jumpConsumed;

    [Header("Jump Buffer")] // A small time to recorder a jump request
    [SerializeField] private float _jumpBufferTime = 0.15f;

    private float _jumpBufferCounter;
    private bool _wasGrounded;

    private void OnEnable()
    {
        InputListner.Instance.Input.Player.Jump.performed += PerformJump;
    }

    private void OnDisable()
    {
        InputListner.Instance.Input.Player.Jump.performed -= PerformJump;
    }

    private void PerformJump(InputAction.CallbackContext context)
    {
        _jumpBufferCounter = _jumpBufferTime;
    }

    public float CalculateVerticalVelocity()
    {
        if ((_characterController.collisionFlags &
            CollisionFlags.Above) != 0 && _verticalVelocity > 0f)
        {
            _verticalVelocity = 0f;
        }

        bool isGrounded = _characterController.isGrounded;

        // Detect landing
        if (isGrounded && !_wasGrounded && _verticalVelocity <= 0f)
        {
            OnHitGround?.Invoke();
        }

        _wasGrounded = isGrounded;

        if (_characterController.isGrounded && _verticalVelocity <= 0f)
        {
            _coyoteTimeCounter = _coyoteTime;

            _verticalVelocity = _groundForce;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            _jumpBufferCounter = 0f;
            _coyoteTimeCounter = 0f;

            OnJump?.Invoke();
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        return _verticalVelocity;
    }
}
