using UnityEngine;
using System.Collections;

public class PoisongasBottle : InteractiveItem
{
    public GameObject Gas;

    public override void Hurt(float damage)
    {
      
        StartCoroutine("HurtColorChange");
        Health -= damage;

        if (Health <= 0)
        {
            Instantiate(Gas, this.transform.position, Quaternion.identity);
            Instantiate(DestroyEffect, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
