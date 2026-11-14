using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Spawn()
    {
        Bomber newBomber = (ObjectPoolManager.Instance.Spawn(PoolsNames.Bomber,transform.position, transform.rotation)).GetComponent<Bomber>();

    }

    private IEnumerator Wait()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(5f);
        }
    }
}
