using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> _pools;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        _pools = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> _pool = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                _pool.Add(obj);
            }
            _pools.Add(pool.tag, _pool);
        }
    }

    public GameObject Spawn(string tag, Vector3Int pos, Quaternion rot)
    {
        if (!_pools.ContainsKey(tag)) return null;
        
        GameObject spawnObj = _pools[tag][0];
        _pools[tag].RemoveAt(0);
        
        spawnObj.SetActive(true);
        spawnObj.transform.position = (Vector3)pos;
        spawnObj.transform.rotation = rot;
        
        _pools[tag].Add(spawnObj);
        return spawnObj;
    }
    public void DeSpawn(string tag, GameObject obj)
    {
        if (!_pools.ContainsKey(tag)) return;
        if(!_pools[tag].Contains(obj)) return;
        
        obj.SetActive(false);
        _pools[tag].Insert(0, obj);
    }

    public void ClearAll()
    {
        foreach (Pool pool in pools)
        {
            List<GameObject> _pool = _pools[pool.tag];
            for (int i = 0; i < _pool.Count; i++)
            {
                _pool[i].SetActive(false);
            }
        }
    }
}
