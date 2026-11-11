using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : Gun
{
    private void SubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Attack.performed += DoShoot;
    }

    private void UnSubscribeToEvents()
    {
        InputListner.Instance.Input.Player.Attack.performed -= DoShoot;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void DoShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }
}
