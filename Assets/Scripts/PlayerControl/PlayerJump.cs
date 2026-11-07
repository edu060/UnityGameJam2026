using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
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
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        return _verticalVelocity;
    }
}
