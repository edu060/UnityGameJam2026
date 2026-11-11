using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public string Id;
        public GameObject Prefab;
        public int InitialSize = 10;
        public int MaxSize = 50;
    }

    [SerializeField]
    private List<PoolConfig> _poolConfigs = new();

    private Dictionary<string, ObjectPool> _pools = new();

    private Dictionary<GameObject, ObjectPool> _activeObjects = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Generate PoolCongigs for use
        foreach (PoolConfig config in _poolConfigs)
        {
            if (string.IsNullOrWhiteSpace(config.Id) ||
                config.Prefab == null)
                continue;

            if (_pools.ContainsKey(config.Id))
            {
                Debug.LogWarning("Duplicated pool: " + config.Id);
                continue;
            }

            _pools.Add(
                config.Id,
                new ObjectPool(
                    config.Prefab,
                    transform,
                    config.InitialSize,
                    config.MaxSize
                )
            );
        }
    }

    public GameObject Spawn(string id, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(id, out ObjectPool pool))
        {
            Debug.LogWarning("Pool not Found: " + id);
            return null;
        }

        GameObject obj = pool.Get(position, rotation);

        _activeObjects[obj] = pool;

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (obj == null)
            return;

        if (!_activeObjects.TryGetValue(obj, out ObjectPool pool))
        {
            Debug.LogWarning("Object doesn't have a pool.");
            return;
        }

        _activeObjects.Remove(obj);
        pool.Release(obj);
    }
}
