using UnityEngine;
using System.Collections;

public class DeadTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("kill player");
            col.gameObject.GetComponent<Player>().Hurt(col.gameObject.GetComponent<Player>().CurrentHealth);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        { 
            BaseEntity entity = col.gameObject.GetComponent<BaseEntity>();
            if(entity != null)
            {
                entity.Hurt(entity.CurrentHealth, entity.transform.position, 1);
            }
            
        }
    }
}
