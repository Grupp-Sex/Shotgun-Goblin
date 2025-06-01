using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOfObjects : MonoBehaviour
{
    private PoolableObject prefab;
    private int size;
    private List<PoolableObject> AvailableObjectsInPool;

    private PoolOfObjects(PoolableObject prefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
        AvailableObjectsInPool = new List<PoolableObject>(size);
    }

    public static PoolOfObjects CreateInstance(PoolableObject prefab, int size)
    {
        PoolOfObjects pool = new PoolOfObjects(prefab, size);

        GameObject GameObjetctForPool = new GameObject(prefab + " Pool");
        pool.CreateObjects(GameObjetctForPool);

        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.parent = this;
            poolableObject.gameObject.SetActive(false);
        }
    }

    public PoolableObject GetObject()
    {
        PoolableObject instanceOfObject = AvailableObjectsInPool[0];

        AvailableObjectsInPool.RemoveAt(0);

        instanceOfObject.gameObject.SetActive(true);

        return instanceOfObject;
    }

    public void AddObjectToPool(PoolableObject Object)
    {
        AvailableObjectsInPool.Add(Object);
    }
}