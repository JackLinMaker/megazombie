using UnityEngine;
using System.Collections;

public class ShootingSaw : MonoBehaviour 
{
    public float TruningSpeed;
    public float Speed;
    public Vector3 Direction;
    public GameObject DestroyEffect;
    public Transform Part1;
    public Transform Part2;

    private int direction;

    private bool isShooting = false;

    void Start()
    {
        
    }

    public void Shooting()
    {
        isShooting = true;
    }

    void Update()
    {
        Part1.Rotate(Vector3.forward * Time.deltaTime * TruningSpeed);
        Part2.Rotate(-Vector3.forward * Time.deltaTime * TruningSpeed);
        if (isShooting)
        {
            transform.Translate(Direction * Speed * Time.deltaTime);
        }
       
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<Player>().Hurt(collider.gameObject.GetComponent<Player>().CurrentHealth, true);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Instantiate(DestroyEffect, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Hit");
            BaseEntity entity = collider.gameObject.transform.GetComponent<BaseEntity>();
            if (entity != null && entity.CurrentHealth > 0)
            {
                
                if (Mathf.Abs(Direction.x) != 0)
                {
                    if (Direction.x > 0)
                    {
                        direction = 1;
                    }
                    else if (Direction.x < 0)
                    {
                        direction = -1;
                    }
                }
                else
                {
                    direction = 1;
                }

                entity.Hurt(entity.CurrentHealth, entity.transform.position, direction);
                Destroy(gameObject);
            }
           
        }
    }
}
