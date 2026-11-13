using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _listOfSpawns;

    public event UnityAction<string> OnEnemyDeath;

    private Dictionary<string, int> _dictionary = new Dictionary<string, int>();

    private void Start()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(5f);
        }
    }

    private void HandleEnemyDied(string enemyType)
    {
        if(_dictionary.ContainsKey(enemyType))
        {
            _dictionary[enemyType]++;
        }
        else
        {
            _dictionary.Add(enemyType, 1);
        }
    }

    private void SpawnEnemy()
    {
        int rndEnemy = Random.Range(0, 0);
        Enemy newEnemy;

        switch (rndEnemy)
        {
            case 0:
                Spawn();
                break;
        }


    }

    private Enemy Spawn()
    {
        int randomSpawn = Random.Range(0, _listOfSpawns.Count);

        Enemy newEnemy = (ObjectPoolManager.Instance.Spawn(PoolsNames.Bomber, _listOfSpawns[randomSpawn].position, _listOfSpawns[randomSpawn].transform.rotation)).GetComponent<Enemy>();

        return newEnemy;
    }

}
