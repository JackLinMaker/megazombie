using UnityEngine;
using System.Collections;

public class DropingBlock : MonoBehaviour 
{

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            if (rigidbody.velocity.y < 0)
            {
                
                Player player = collider.gameObject.transform.GetComponent<Player>();

                player.InstantKill();
            }

        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
           
            rigidbody.isKinematic = true;
        }
    }
	
}
