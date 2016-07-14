using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour
{


    public Transform DestoryEffect;
    public bool IsFacingRight { get; set; }

   
    public float Speed;
   
    public float Damage;
   
    public float Range;
    
    public Util.ObjectOwner Owner;
    
    public string Name;
   
    public Vector3 Direction;
    
    public Transform Host;

    
    public float HitForceX;
    
    public float HitForceY;

    protected float distance;

    void Update()
    {


        float deltaMovement = Speed * Time.deltaTime;

        transform.Translate(Direction * deltaMovement);
        distance += deltaMovement;

        if (Mathf.Abs(distance) >= Range)
        {
            DestoryWithEffect();
        }
    }

    public virtual void DestoryWithEffect()
    {
        distance = 0.0f;

        Transform desEffect = Instantiate(DestoryEffect, this.transform.position, Quaternion.identity) as Transform;
        desEffect.localRotation = Quaternion.Euler(new Vector3(0, 0, IsFacingRight ? 0 : 180));
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            DestoryWithEffect();
        }
    }
}
