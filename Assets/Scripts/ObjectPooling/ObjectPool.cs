using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly Stack<GameObject> _availableObjects;
    private readonly int _maxSize;

    public ObjectPool(
        GameObject prefab,
        Transform parent,
        int initialSize,
        int maxSize)
    {
        this._prefab = prefab;
        this._parent = parent;
        this._maxSize = maxSize;

        _availableObjects = new Stack<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateObject();
            Release(obj);
        }
    }

    private GameObject CreateObject()
    {
        GameObject obj = UnityEngine.Object.Instantiate(_prefab, _parent);

        obj.SetActive(false);

        return obj;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = (_availableObjects.Count > 0) ? _availableObjects.Pop() : CreateObject();

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawned();

        return obj;
    }

    public void Release(GameObject obj)
    {
        if (obj == null)
            return;

        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnDespawned();

        obj.SetActive(false);

        if (_availableObjects.Count >= _maxSize)
        {
            UnityEngine.Object.Destroy(obj);
            return;
        }

        obj.transform.SetParent(_parent);
        _availableObjects.Push(obj);
    }
}
