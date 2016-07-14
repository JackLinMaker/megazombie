using UnityEngine;
using System.Collections;

public class Flagpole : MonoBehaviour 
{

   

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            
            BaseBullet bullet = collider.gameObject.transform.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER)
            {
                bullet.DestoryWithEffect();
                Debug.Log("hit pole");
                
                Director.Instance.Player.PickupGold(10);
                transform.parent.GetComponent<EndMark>().Win();
            }

        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        { 
           
            Debug.Log("hit pole");
           
            Director.Instance.Player.PickupGold(10);
            transform.parent.GetComponent<EndMark>().Win();

        }
    }

   
	
}
