using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour 
{
    public enum BoundsBehavior
    { 
        Nothing,
        Constrain,
        Kill,
    }

    public BoxCollider2D Bounds;
    public BoundsBehavior Above;
    public BoundsBehavior Below;
    public BoundsBehavior Left;
    public BoundsBehavior Right;

    private Player player;
    private BoxCollider2D boxCollider;


    public void Start()
    {
        player = GetComponent<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (player.IsDead || Bounds)
            return;

        var colliderSize = new Vector2(boxCollider.size.x * Mathf.Abs(transform.localScale.x), boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
        
        if (Above != BoundsBehavior.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
            ApplyBoundsBehavior(Above, new Vector2(transform.position.x, transform.position.y - colliderSize.y));


        if (Below != BoundsBehavior.Nothing && transform.position.y - colliderSize.y < Bounds.bounds.min.y)
            ApplyBoundsBehavior(Below, new Vector2(transform.position.x, transform.position.y + colliderSize.y));

        if (Right != BoundsBehavior.Nothing && transform.position.x + colliderSize.x > Bounds.bounds.max.x)
            ApplyBoundsBehavior(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y));


        if (Left != BoundsBehavior.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
            ApplyBoundsBehavior(Left, new Vector2(Bounds.bounds.min.x + colliderSize.x, transform.position.y));

    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainPosition)
    {
        if (behavior == BoundsBehavior.Kill)
        { 
            // kill the player
            return;
        }

        transform.position = constrainPosition;
    }
	
}
