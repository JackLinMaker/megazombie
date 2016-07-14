using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainPool : MonoBehaviour
{

    public static RainPool Instance;
    public GameObject PooledObject;
    public int PooledAmount = 30;
    public bool WillGrow = true;

    private List<GameObject> pooledObjects;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < PooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(PooledObject);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        StartCoroutine(Rain());
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
            obj.transform.parent = this.transform;
            return obj;
        }
        return null;
    }

    public IEnumerator Rain()
    {
        while (true)
        {
            GameObject rain = GetPooledObject();
            rain.transform.position = transform.position + new Vector3(Random.Range(-7, 7), 0, 0);
            rain.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(5, -5)));
            rain.gameObject.SetActive(true);

            rain.GetComponent<Rain>().CloseRain();
            yield return new WaitForSeconds(0.01f);
        }


    }
}
