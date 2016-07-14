using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{

    public float Damage;
    public float DamageRadius = 1f;
    public GameObject ExplosionEffect;
    public Vector3 Target { get; set; }
    public LayerMask DamageMask;
    public float Vy;

    private bool isLanding;
    private float Duration;
    private float Gravity = 9.8f;
    private float Vx;
    private Rigidbody2D rigidbody2D;
    private CircleCollider2D collider2D;
     
 
    void Awake()
    {
        isLanding = false;
        collider2D = GetComponent<CircleCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.angularVelocity = 720.0f;
    }

    

    /*public void StartMove()
    {
        float distanceX = Target.x - transform.position.x;
        float distanceY = Target.y - transform.position.y;
        float time_to_top = Vy / Gravity;
       
        float jump_height = Vy * time_to_top - Gravity * time_to_top * time_to_top * 0.5f;


        float falling_height = Mathf.Abs(jump_height - distanceY);
     
        
        float time_to_fall = Mathf.Sqrt(2 * falling_height / Gravity);
       
        float total_time = time_to_top + time_to_fall;
        Duration = total_time;

        Vx = distanceX / total_time;
       
        
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        float elapsed = 0.0f;
        while (!isLanding)
        {
            transform.Translate(Vx * Time.deltaTime, (Vy - (Gravity * elapsed)) * Time.deltaTime, 0.0f, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }*/

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("tag = " + collider.gameObject.tag);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            
            explosion();
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            explosion();
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
           
            if (collider.gameObject.GetComponent<BaseBullet>().Owner == Util.ObjectOwner.PLAYER)
            {
                Director.Instance.ShakeScreen();
                Instantiate(ExplosionEffect, this.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }
        
    }

    private IEnumerator bombOngroundCo()
    {
        yield return new WaitForSeconds(0.4f);
        explosion();
    }

    private void explosion()
    {
        //Director.Instance.ShakeScreen();
        GameObject landingEffect = Instantiate(ExplosionEffect, this.transform.position, Quaternion.identity) as GameObject;
        Collider2D[] casts = Physics2D.OverlapCircleAll(this.transform.position, DamageRadius, DamageMask);
        for (int i = 0; i < casts.Length; i++)
        {

            if (casts[i].transform.GetComponent<Player>() != null)
            {
                Player player = casts[i].transform.GetComponent<Player>();
                player.Hurt(Damage, rigidbody2D.velocity.x >= 0 ? true : false);
                Debug.Log("vx = " + rigidbody2D.velocity.x);
                player.HitBack(rigidbody2D.velocity.x >= 0 ? 10.0f : -10.0f, 10.0f);
            }
            else if (casts[i].transform.GetComponent<InteractiveItem>() != null)
            {
                InteractiveItem item = casts[i].transform.GetComponent<InteractiveItem>();
                item.Hurt(Damage);
            }
            else if (casts[i].transform.GetComponent<BaseEntity>() != null)
            {
                BaseEntity entity = casts[i].transform.GetComponent<BaseEntity>();
                entity.Hurt(Damage, entity.transform.position, Vx >= 0 ? 1 : -1);
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator flashRedCo()
    {
        int count = 8;
        for (int i = 0; i < count; i++)
        {
            this.transform.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.5f);
            this.transform.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.red);
            yield return new WaitForSeconds(0.1f);
            this.transform.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }

   
}
