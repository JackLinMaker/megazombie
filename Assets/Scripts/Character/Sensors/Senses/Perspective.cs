using UnityEngine;
using System.Collections;

public class Perspective : Sense
{
    public float faceRayDistance = 2f;
    public float backRayDistance = 1f;
    public LayerMask playerMask = 0;
    public Vector3 direction;
    
    private Vector3 rayDirection;
    private Vector3 faceRayOrigin;
    private BoxCollider2D collider;
    private Vector2 center;
    private Vector2 size;
    private int totalHorizontalRays;
    private float verticalDistanceBetweenRays;
    private Transform animator;
    private CharacterController2D controller;
    private BaseEntity entity;
    
    private bool detected;

    public bool Detected
    {
        get
        {
            return detected;
        }
        set
        {
            detected = value;
            
        }
    }
    
    protected override void Initialize()
    {
        detected = false;
        collider = transform.GetComponent<BoxCollider2D>();
        size = collider.size;
        center = collider.offset;
        animator = this.transform.Find("Animator");
        totalHorizontalRays = 8;
        verticalDistanceBetweenRays = size.y / (totalHorizontalRays - 1);
        controller = GetComponent<CharacterController2D>();
        entity = GetComponent<BaseEntity>();
    }

    

    protected override void UpdateSense()
    {
       
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= detectionRate)
        {
            
            detectAspect();
        }
    }

    void detectAspect()
    {
        var rayDirection = new Vector3(animator.transform.localScale.x, 0, 0);
        faceRayOrigin = rayDirection.x > 0 ? controller.raycastOrigins.bottomRight : controller.raycastOrigins.bottomLeft;
        for (int i = 0; i < totalHorizontalRays; i++)
        {
            var ray = new Vector2(faceRayOrigin.x, faceRayOrigin.y + i * verticalDistanceBetweenRays);
            if (bDebug)
            {
                DrawRay(ray, rayDirection * faceRayDistance, Color.red);
            }
            
            RaycastHit2D raycastHit = Physics2D.Raycast(ray, rayDirection, faceRayDistance, playerMask);
            if (raycastHit.collider != null)
            {
                
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    
                    Player player = raycastHit.collider.gameObject.transform.GetComponent<Player>();
                    if (player.CurrentHealth > 0)
                    {
                        detected = true;
                        entity.Target = raycastHit.collider.gameObject.transform;

                    }
                    break;
                }
                
            }
        }

        var backRayOrigin = rayDirection.x > 0 ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
        for (int i = 0; i < totalHorizontalRays; i++)
        {
            var ray = new Vector2(backRayOrigin.x, backRayOrigin.y + i * verticalDistanceBetweenRays);
            if (bDebug)
            {
                DrawRay(ray, -rayDirection * backRayDistance, Color.red);
            }
            
            RaycastHit2D raycastHit = Physics2D.Raycast(ray, -rayDirection, backRayDistance, playerMask);
            if (raycastHit.collider != null)
            {
                
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    
                    Player player = raycastHit.collider.gameObject.transform.GetComponent<Player>();
                    if (player.CurrentHealth > 0)
                    {

                        detected = true;
                        entity.Target = raycastHit.collider.gameObject.transform;

                    }
                    break;
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        
    }

    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        
        Debug.DrawRay(start, dir, color);
    }
    

}
