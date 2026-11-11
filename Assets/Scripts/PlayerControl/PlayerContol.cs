using UnityEngine;

public class PlayerContol : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerRotation _playerRotation;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private PlayerGun _playerGun;

    private void Start()
    {
        _playerMovement.enabled = true;
        _playerRotation.enabled = true;
        _playerJump.enabled = true;
        _playerGun.enabled = true;
    }

}
