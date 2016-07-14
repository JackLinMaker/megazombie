using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour
{
    public float Speed;
    public Transform Part1;
    public Transform Part2;
    public float Damage;

    void Update()
    {
        Part1.Rotate(Vector3.forward * Time.deltaTime * Speed);
        Part2.Rotate(-Vector3.forward * Time.deltaTime * Speed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collider.gameObject.GetComponent<Player>();
            if (!player.IsHurt)
            {
                player.Hurt(50.0f, player.Controller.Velocity.x >= 0 ? false : true);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collider.gameObject.GetComponent<Player>();
            if (!player.IsHurt)
            {
                player.Hurt(Damage, player.Controller.Velocity.x >= 0 ? false : true);
            }
        }
    }
}
