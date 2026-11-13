using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public readonly Dictionary<SoundData, int> Counts = new Dictionary<SoundData, int>();

    [SerializeField] private SoundEmitter _soundEmitterPrefab;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private int _maxSoundInstances = 30;

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public SoundEmitter Get()
    {
        GameObject obj = ObjectPoolManager.Instance.Spawn(PoolsNames.Bullet, transform.position, transform.rotation);
        return obj.GetComponent<SoundEmitter>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
