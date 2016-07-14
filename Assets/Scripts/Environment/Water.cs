using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    public GameObject WaterSplashEffect;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(WaterSplashEffect, col.gameObject.transform.position, Quaternion.identity);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(WaterSplashEffect, col.gameObject.transform.position, Quaternion.identity);
        }
    }
}
