using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<int, Queue<PoolObject>> poolDictionary = new Dictionary<int, Queue<PoolObject>>();
    private List<PoolObject> pooledObjects = new List<PoolObject>();
    //private Queue<PoolObject> pooledObjects = new Queue<PoolObject>();

    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PoolManager>();
            return _instance;
        }
    }



    private PoolObject CreatePoolObject(PoolObject prefab, int poolId)
    {
        PoolObject poolObject = Instantiate(prefab) as PoolObject;
        poolObject.poolid = poolId;
        poolObject.gameObject.SetActive(false);
        return poolObject;
    }

    public void CreatePool(PoolObject prefab, int poolSize)
    {
        int id = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(id))
        {
            poolDictionary.Add(id, new Queue<PoolObject>());

            for (int i = 0; i < poolSize; i++)
            {
                poolDictionary[id].Enqueue(CreatePoolObject(prefab, id));
            }
        }
    }

    public void NotUsedObject(PoolObject obj)
    {
        obj.gameObject.SetActive(false);
        poolDictionary[obj.poolid].Enqueue(obj);
        pooledObjects.Remove(obj);
    }

    
    public void NotUsedObjects(float playerZ)
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (playerZ * 120 - pooledObjects[i].transform.position.z > 21f )
            {
                pooledObjects[i].gameObject.SetActive(false);
                poolDictionary[pooledObjects[i].poolid].Enqueue(pooledObjects[i]);
                pooledObjects.Remove(pooledObjects[i]);
                i--;
            }
        }
        //Debug.Log(pooledObjects.Count);
    }


    public PoolObject UseObject(PoolObject prefab, Vector3 position, Quaternion rotation, bool setActive = true)
    {
        int id = prefab.GetInstanceID();
        
        //Controls if dictionary have created pool. if not creates a new one
        if (!poolDictionary.ContainsKey(id))
        {
            CreatePool(prefab, 1);
        }

        //Controls if pool have object that can be used. if not creates new poolobject
        if (poolDictionary[id].Count == 0)
        {
            poolDictionary[id].Enqueue(CreatePoolObject(prefab, id));
            
        }

        PoolObject gObject = poolDictionary[id].Dequeue();
        pooledObjects.Add(gObject);
        if (gObject.GetComponent<Rigidbody>() != null)
        {
            gObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        
        gObject.transform.position = position;
        gObject.transform.rotation = rotation;
        gObject.gameObject.SetActive(setActive ? true : false);
        return gObject;
    }
}
