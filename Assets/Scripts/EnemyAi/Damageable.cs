using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public abstract void TakeDamage(float amount, GameObject source);
}
