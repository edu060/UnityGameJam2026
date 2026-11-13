using UnityEngine;

public class ImpProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float projectileDamage = 50f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable target = other.GetComponentInParent<Damageable>();

        if (target != null)
        {
            target.TakeDamage(projectileDamage, gameObject);
        }

        Destroy(gameObject);
    }
}
