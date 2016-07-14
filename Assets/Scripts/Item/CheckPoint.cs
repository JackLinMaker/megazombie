using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
    public GameObject Flag;
    public Transform Destination;
    public float Speed;

    private BoxCollider2D boxCollider;
    private bool isMoving;
    

    void Awake()
    {
        isMoving = false;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collider.gameObject.transform.GetComponent<Player>().CurrentHealth > 0)
            {

                isMoving = true;
                boxCollider.enabled = false;
                Director.Instance.HitCheckPoint();
            }
           
        }
    }

    void Update()
    {
        if (isMoving && Flag.transform.position.y <= Destination.position.y)
        {
            Flag.transform.Translate(Vector2.up * Time.deltaTime * Speed);
        }
        
    }

   


	
}
