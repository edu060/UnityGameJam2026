using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    private float _speed;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void OnSpawned()
    {
        IEnumerator Despawn()
        {
            yield return new WaitForSeconds(3f);
            ObjectPoolManager.Instance.Despawn(gameObject);
        }

        StartCoroutine(Despawn());
    }

    public void OnDespawned()
    {
        _speed = 0;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
    }

    private void Update()
    {
        MoveForward();
    }
}
