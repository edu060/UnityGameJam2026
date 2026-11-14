using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnerManager : MonoBehaviour
{
    public UnityAction OnEnemyDeath;

    [Header("Enemies Control")]
    [Tooltip("All the places where enemies can spawn")]
    [SerializeField] private List<Transform> _listOfSpawns;
    [Tooltip("How many enemies can have on the scene")]
    [SerializeField] private int _maxEnemies = 20;
    [Tooltip("How mouch time (in seconds) is the space between enimes spawns")]
    [SerializeField] private float _spawnSpacing = 2f;
    [Tooltip("How many enemies can spawn on a row")]
    [SerializeField] private int _maxEnemiesSpawnedPerRow = 3;
    [Tooltip("If enemies can spawn at same spawnpoint or not (false = can't spawn on the same spot)\n" +
        "Note: If MaxEnemiesSpawnedPerRow is greater than ListOfSpawns, this will be ignored")]
    [SerializeField] private bool _samePlaceSpawning = false;

    private int _enemiesAmount;
    private int _enemiesKilledAmount;

    private List<int> _spawnsIndexes = new List<int>(); 
    private int _currentSpawnIndex = 0;

    private string _enemyToSpawn = "";

    public void NotifyDeath(string enemyName)
    {
        _enemiesAmount--;
        _enemiesKilledAmount++;

        OnEnemyDeath?.Invoke();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return null;

        while (true)
        {
            yield return new WaitUntil (() => (_enemiesAmount < _maxEnemies));
            yield return new WaitForSeconds(_spawnSpacing);

            if (_samePlaceSpawning == false && _listOfSpawns.Count >= _maxEnemiesSpawnedPerRow)
            {
                _currentSpawnIndex = 0;
                GenerateListOfSpawnpointIndexes();
            }

            int enemiesToSpawn = Mathf.Min(
                _maxEnemiesSpawnedPerRow,
                _maxEnemies - _enemiesAmount
                );

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnRandom();
            }
        }
    }

    private void SpawnRandom()
    {
        int rndEnemy = Random.Range(0, 3);

        switch (rndEnemy)
        {
            case 0:
                _enemyToSpawn = PoolsNames.Bird;
                break;
            case 1:
                _enemyToSpawn = PoolsNames.Imp;
                break;
            case 2:
                _enemyToSpawn = PoolsNames.Bomber;
                break;
            default:
                _enemyToSpawn = PoolsNames.Imp;
                break;
        }
        
        Spawn();
    }

    private Enemy Spawn()
    {
        int randomSpawn = GetSpawnPointIndex();

        Enemy newEnemy = (ObjectPoolManager.Instance.Spawn(_enemyToSpawn, _listOfSpawns[randomSpawn].position, _listOfSpawns[randomSpawn].transform.rotation)).GetComponent<Enemy>();

        newEnemy.SetSpawnManager(this);
        _enemiesAmount++;

        return newEnemy;
    }

    private int GetSpawnPointIndex()
    {
        if(_samePlaceSpawning == false && _listOfSpawns.Count >= _maxEnemiesSpawnedPerRow)
        {
            _currentSpawnIndex++;
            return _spawnsIndexes[_currentSpawnIndex - 1];
        }
        else
        {
            return Random.Range(0, _listOfSpawns.Count);
        }
    }

    private void GenerateListOfSpawnpointIndexes()
    {
        _spawnsIndexes.Clear();

        int amount = Mathf.Min(
            _maxEnemiesSpawnedPerRow,
            _listOfSpawns.Count
        );

        while (_spawnsIndexes.Count < amount)
        {
            int index = Random.Range(0, _listOfSpawns.Count);

            if (!_spawnsIndexes.Contains(index))
                _spawnsIndexes.Add(index);
        }
    }
}
