using UnityEngine;
using System.Collections;

public class Missile : BaseBullet
{
    public Transform Target;
    public float MissileRotateSpeed = 2f;
    public float DestroyTime;
    private float timer;
    
    void Start()
    {
        timer = 0;
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > DestroyTime)
        {
           
            DestoryWithEffect();
        }


        /*Vector3 targetDirection = (Target.transform.position + new Vector3(0, 0.5f, 0)) - transform.position;
        float angle = Vector3.Angle(transform.right * (transform.localScale.x == 1 ? 1 : -1), targetDirection);
        if (angle > 30)
        {
            transform.Rotate(Vector3.forward * (transform.localScale.x == 1 ? -1 : 1), angle * Time.deltaTime * 2);
        }
        transform.Translate(Vector3.right * (transform.localScale.x == 1 ? 1 : -1) * Time.deltaTime * Speed);*/

        
        Vector3 targetDirection = Target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        Debug.Log("targetRotation = " + targetRotation);
        Debug.Log("transform rotation = " + transform.rotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * Speed);
        transform.position += transform.forward * Speed * Time.deltaTime;
            
    }

    public override void DestoryWithEffect()
    {
        Transform desEffect = Instantiate(DestoryEffect, this.transform.position, Quaternion.identity) as Transform;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DestoryWithEffect();
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (collider.gameObject.transform.GetComponent<BaseBullet>().Owner == Util.ObjectOwner.PLAYER)
            {
                DestoryWithEffect();
            }
        }
    }
}
