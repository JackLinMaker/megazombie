using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour 
{
    public Spawner spanwer;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            spanwer.StartSpawn();
        }
    }
}
