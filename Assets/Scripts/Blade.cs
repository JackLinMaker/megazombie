using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;
    public int Damage;

   

    public void SetColliderForSprite(int spriteNum)
    {
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = spriteNum;
        colliders[currentColliderIndex].enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
       
    }

    

   

}
