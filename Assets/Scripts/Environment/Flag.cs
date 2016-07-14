using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour 
{

   
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.gameObject.transform.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER)
            {
                bullet.DestoryWithEffect();
                Debug.Log("hit flag");
                
                Director.Instance.Player.PickupGold(100);
                transform.parent.GetComponent<EndMark>().Win();
                
            }
        }
    }

   
	
}
