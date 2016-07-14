using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour 
{
    //public GameObject collectableEffect;

    public enum CollectableType
    { 
        Key,
        Hp,
        Gold,
    }

    public CollectableType Type;
    public int Count;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collider.gameObject.transform.GetComponent<Player>();
           
            if (Type == CollectableType.Key)
            {
                player.PickUpKey(Count);
            }
            else if (Type == CollectableType.Hp)
            {
                player.PickupHp(Count);

            }
            else if (Type == CollectableType.Gold)
            {
                player.PickupGold(Count);
            }
         
            Destroy(this.gameObject);
           
        }

    }
}
