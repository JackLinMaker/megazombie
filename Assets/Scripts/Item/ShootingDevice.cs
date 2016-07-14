using UnityEngine;
using System.Collections;

public class ShootingDevice : MonoBehaviour 
{
    public ShootingSaw Saw;
    public Vector3 Direction;
    public Transform ShootingPoint;
    public float SpawnTime;
    public float Speed;

    private Animator animator;
    private ShootingSaw entity;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        StartCoroutine(ProduceSaw());
    }

    IEnumerator ProduceSaw()
    {
        entity = Instantiate(Saw, ShootingPoint.position, Quaternion.identity) as ShootingSaw;
        entity.Direction = Direction;
        entity.Speed = Speed;
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("Shooting", true);
    }


    public void StartShooting()
    {
        Debug.Log("StartShooting");
        entity.Shooting();
    }

    public void FinishShooting()
    {
        Debug.Log("FinishShooting");
        animator.SetBool("Shooting", false);
        StartCoroutine(ProduceSaw());
    }

    
}
