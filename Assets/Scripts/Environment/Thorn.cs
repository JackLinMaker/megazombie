using UnityEngine;
using System.Collections;

public class Thorn : MonoBehaviour {

    public int Damage;

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<Player>().Hurt(collider.gameObject.GetComponent<Player>().CurrentHealth, true);
        }

    }
}
