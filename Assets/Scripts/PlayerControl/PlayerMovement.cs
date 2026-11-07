using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementController
{
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PlayerJump _playerJump;

    private Vector2 _inputDirection;

    private void OnEnable()
    {
        CanMove = true;
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    protected override void DoMovement()
    {
        if (!CanMove)
            return;

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Direction = right * _inputDirection.x + forward * _inputDirection.y;

        Vector3 horizontalVelocity = Direction * MovementSpeed;

        float verticalVelocity = _playerJump.CalculateVerticalVelocity();

        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        _characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void SubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Move.performed += CalculateMovement;
        InputListner.Instance.Input.Player.Move.canceled += StopPlayerMovement;
    }

    private void UnSubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Move.performed -= CalculateMovement;
        InputListner.Instance.Input.Player.Move.canceled -= StopPlayerMovement;
    }

    private void CalculateMovement(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void StopPlayerMovement(InputAction.CallbackContext context)
    {
        _inputDirection = Vector2.zero;
    }
}
