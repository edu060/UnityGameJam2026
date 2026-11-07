using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprint : MonoBehaviour
{
    [Header("Sprint Settings")]
    [SerializeField] private float _sprintMultiplier = 1.5f;

    private bool _isSprinting;

    public bool IsSprinting => _isSprinting;

    private void OnEnable()
    {
        InputListner.Instance.Input.Player.Sprint.performed += StartSprint;
        InputListner.Instance.Input.Player.Sprint.canceled += StopSprint;
    }

    private void OnDisable()
    {
        InputListner.Instance.Input.Player.Sprint.performed -= StartSprint;
        InputListner.Instance.Input.Player.Sprint.canceled -= StopSprint;

        _isSprinting = false;
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        _isSprinting = true;
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        _isSprinting = false;
    }

    public float GetSpeed(float baseSpeed)
    {
        return _isSprinting
            ? baseSpeed * _sprintMultiplier
            : baseSpeed;
    }
}
