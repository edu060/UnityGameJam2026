using UnityEngine;
using UnityEngine.U2D.Animation;

public class BillboardCharacter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _directionReference;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteResolver _spriteResolver;

    private void LateUpdate()
    {
        UpdateSprite();
        UpdateBillboard();
    }

    private void Awake()
    {
        if(_camera == null)
        {
            _camera = Camera.main.transform;
        }
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
            _spriteResolver.SetCategoryAndLabel(OrientationData.Front, _spriteResolver.GetLabel());
        }
        else if (angle < 90f)
        {
            _spriteResolver.SetCategoryAndLabel(OrientationData.FrontRight, _spriteResolver.GetLabel());
        }
        else if (angle < 150f)
        {
            _spriteResolver.SetCategoryAndLabel(OrientationData.BackRight, _spriteResolver.GetLabel());
        }
        else if (angle < 210f)
        {
            _spriteResolver.SetCategoryAndLabel(OrientationData.Back, _spriteResolver.GetLabel());
        }
        else if (angle < 270f)
        {
            _spriteResolver.SetCategoryAndLabel(OrientationData.BackLeft, _spriteResolver.GetLabel());
        }
        else
        {
            _spriteResolver.SetCategoryAndLabel(OrientationData.FrontLeft, _spriteResolver.GetLabel());
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
