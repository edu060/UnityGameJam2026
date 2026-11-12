using UnityEngine;
using UnityEngine.U2D.Animation;

public class BillboardCharacter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _directionReference;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteResolver _spriteResolver;

    [Header("Sprites")]
    [SerializeField] private CharacterSprites _sprites;

    private void LateUpdate()
    {
        UpdateSprite();
        UpdateBillboard();
    }

    private void UpdateSprite()
    {
        Vector3 direction =
            _camera.position - _directionReference.position;

        Vector3 localDirection =
            _directionReference.InverseTransformDirection(direction);

        localDirection.y = 0f;

        float angle = Mathf.Atan2(
            localDirection.x,
            localDirection.z
        ) * Mathf.Rad2Deg;

        angle = (angle + 360f) % 360f;

        if (angle < 30f || angle >= 330f)
        {
            _spriteRenderer.sprite = _sprites.Front;
        }
        else if (angle < 90f)
        {
            _spriteRenderer.sprite = _sprites.FrontLeft;
        }
        else if (angle < 150f)
        {
            _spriteRenderer.sprite = _sprites.BackLeft;
        }
        else if (angle < 210f)
        {
            _spriteRenderer.sprite = _sprites.Back;
        }
        else if (angle < 270f)
        {
            _spriteRenderer.sprite = _sprites.BackRight;
        }
        else
        {
            _spriteRenderer.sprite = _sprites.FrontRight;
        }
    }

    private void UpdateBillboard()
    {
        Vector3 direction =
            _camera.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }
}
