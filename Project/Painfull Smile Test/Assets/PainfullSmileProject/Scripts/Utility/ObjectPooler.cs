using NekraByte;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region - Singleton Pattern -
    private static ObjectPooler _instance;
    public static ObjectPooler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPooler>();

                if (_instance == null)
                {
                    GameObject singletonInstance = new GameObject(typeof(ObjectPooler).Name);
                    _instance = singletonInstance.AddComponent<ObjectPooler>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    [SerializeField] private List<PoolData> poolDatas = new List<PoolData>();

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolData pool in poolDatas)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("This pool is invalid, returning the code.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        if (objectToSpawn == null) 
            return null;

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPolled pooledObj = objectToSpawn.GetComponent<IPolled>();

        if (pooledObj != null)
            pooledObj.Pooled();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}