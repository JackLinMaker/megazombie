using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public float spawnTime = 5f;

    public GameObject[] entities;

    private int index = 0;
    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void StartSpawn()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(spawnTime);
       
        if (index < entities.Length)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - renderer.bounds.size.y * 0.5f, transform.position.z);

            GameObject spawnObject = Instantiate(entities[index], pos, transform.rotation) as GameObject;
            BaseEntity entity = spawnObject.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.Spawn(Random.Range(-1, 1));
                index++;
                StartCoroutine(spawn());
            }
            
        }
    }
	
}
