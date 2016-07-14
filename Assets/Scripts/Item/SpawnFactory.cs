using UnityEngine;
using System.Collections;

public class SpawnFactory : MonoBehaviour
{

    public GameObject Zombie;
    public GameObject Target;
    // Update is called once per frame
    public float SpawnTime = 2f;
    public int EnemyCount = 10;

    private bool isSpawn = true;
    private int index = 0;
    private float timer;

    void Start()
    {
        timer = SpawnTime;
    }

    void Update()
    {
        if (isSpawn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = SpawnTime;
                index++;
                GameObject zombie = Instantiate(Zombie, transform.position, Quaternion.identity) as GameObject;
                zombie.GetComponent<BaseEntity>().Target = Target.transform;
                zombie.GetComponent<Perspective>().Detected = true;
                if (index == EnemyCount)
                {
                    isSpawn = false;
                }
            }

        }
    }
}
