using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool Instance;
    public GameObject PooledObject;
    public int PooledAmount = 10;
    public bool WillGrow = true;

    private List<GameObject> pooledObjects;

    void Awake()
    {
        Instance = this;
    }

	void Start () 
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < PooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(PooledObject);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
	}

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (WillGrow)
        {
            GameObject obj = (GameObject)Instantiate(PooledObject);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
