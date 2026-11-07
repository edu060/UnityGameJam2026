using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _cameraPivot;

    [Header("Rotation Settings")]
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _minPitch = -80f;
    [SerializeField] private float _maxPitch = 80f;

    private float _pitch;

    private void OnEnable()
    {
        InputListner.Instance.Input.Player.Look.performed += CalculateRotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        InputListner.Instance.Input.Player.Look.performed -= CalculateRotation;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CalculateRotation(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        float mouseX = input.x * _sensitivity;
        float mouseY = input.y * _sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
