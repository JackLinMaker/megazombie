#define DEBUG_CC2D_RAYS
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    #region internal types

    public struct CharacterRaycastOrigins
    {
        public Vector3 topRight;
        public Vector3 topLeft;
        public Vector3 bottomRight;
        public Vector3 bottomLeft;
    }

    public class CharacterCollisionState2D
    {
        public bool Right;
        public bool Left;
        public bool Above;
        public bool Below;
        public bool BecameGroundedThisFrame;
        public bool IsMovingUpSlope;
        public bool IsMovingDownSlope;
        public float SlopeAngle;
        public bool IsHittingWall;
        public int FaceDirection;


        public bool HasCollision()
        {
            return Below || Right || Left || Above;
        }

        public void Reset()
        {
            Right = Left = Above = Below = BecameGroundedThisFrame = IsMovingUpSlope = IsMovingDownSlope = IsHittingWall = false;
            SlopeAngle = 0f;
        }


        public override string ToString()
        {
            return string.Format("[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, movingUpSlope: {4}, angle: {5}",
                                 Right, Left, Above, Below, IsMovingUpSlope, IsMovingDownSlope);
        }
    }

    #endregion

    #region properties and fields
    public bool HandlePlatforms { get; set; }

    public ControllerParameters2D DefaultParameters;
    public LayerMask PlatformMask = 0;
    public LayerMask OneWayPlatformMask = 0;
    public Vector2 Velocity { get { return velocity; } }
    public Vector2 PreVelocity { get { return preVelocity; } }
    public Animator Animator { get { return animator; } }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return overrideParameters ?? DefaultParameters; } }
    public GameObject StandingOn { get; private set; }
    public Vector3 PlatformVelocity { get; private set; }
    public bool IsGrounded { get { return collisionState.Below; } }
    public bool IsHittingWall { get { return collisionState.Left || collisionState.Right; } }
    public CharacterRaycastOrigins raycastOrigins;
    public CharacterCollisionState2D collisionState = new CharacterCollisionState2D();
    public float DeltaMovementX;

    private static readonly float slopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);//75
    private float skinWidth = 0.02f;
    private int totalHorizontalRays = 8;
    private int totalVerticalRays = 4;
    private float verticalDistanceBetweenRays;
    private float horizontalDistanceBetweenRays;
    private bool isfalling = false;
    private Vector2 velocity;
    private Vector2 preVelocity;
    private Vector2 dragForce = Vector2.zero;
    private Vector3 localScale;
    private ControllerParameters2D overrideParameters;
    private GameObject lastStandingOn;
    private Vector3 activeGlobalPlatformPoint;
    private Vector3 activeLocalPlatformPoint;
    private const float kSkinWidthFloatFudgeFactor = 0.0001f;//0.0001f
    #endregion

    #region component
    private Animator animator;
    private Transform transform;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    #endregion

    void Awake()
    {

        HandleCollisions = true;
        HandlePlatforms = true;
        PlatformMask |= OneWayPlatformMask;
        transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        this.animator = this.transform.Find("Animator").gameObject.GetComponent<Animator>();
        localScale = animator.transform.localScale;
        recalculateDistanceBetweenRays();
        collisionState.FaceDirection = 1;
    }

    private void recalculateDistanceBetweenRays()
    {
        // figure out the distance between our rays in both directions
        // horizontal
        var colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2f * skinWidth);
        verticalDistanceBetweenRays = colliderUseableHeight / (totalHorizontalRays - 1);

        // vertical
        var colliderUseableWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2f * skinWidth);
        horizontalDistanceBetweenRays = colliderUseableWidth / (totalVerticalRays - 1);
    }

    #region Public

    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    public void SetForce(Vector2 force)
    {

        velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
        velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        velocity.y = y;
    }

    public void Jump()
    {
        SetForce(new Vector2(0, Parameters.JumpMagnitude));
    }

    public void Impulse()
    {
        Debug.Log("Impulse");
        AddForce(new Vector2(0, Parameters.JumpMagnitude * 0.5f));//0.05f
    }

    public void SetDragForce(Vector2 drag)
    {
        dragForce = drag;
    }

    public void MoveBack(int direction, float deltaMovement, float speed)
    {
        if (direction == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(deltaMovement, 0.0f, 0.0f), speed);
        }
        else if (direction == -1)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-deltaMovement, 0.0f, 0.0f), speed);
        }
    }

    public void Fall()
    {

        if (StandingOn && StandingOn.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            PlatformMask.value = 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Platform");
            isfalling = true;
        }


    }

    public void LateUpdate()
    {
        preVelocity = velocity;
        velocity.y += Parameters.Gravity * Time.deltaTime;
        velocity += dragForce;
        Move(velocity * Time.deltaTime);
    }

    public void Move(Vector2 deltaMovement)
    {
        var wasGroundedBeforeMoving = collisionState.Below;

        collisionState.Reset();

        if (isfalling)
        {
            for (var i = 0; i < totalVerticalRays; i++)
            {
                var ray = new Vector2(raycastOrigins.bottomLeft.x + i * horizontalDistanceBetweenRays, raycastOrigins.bottomLeft.y - 0.025f);
                LayerMask mask = new LayerMask();
                mask.value = 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Platform") | 1 << LayerMask.NameToLayer("OneWayPlatform");
                RaycastHit2D raycastHit = Physics2D.Raycast(ray, -Vector2.up, Mathf.Abs( velocity.y) * 0.1f, mask);

                if (raycastHit)
                {
                    GameObject gg = raycastHit.collider.gameObject;
                    isfalling = false;
                    PlatformMask.value = 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Platform") | 1 << LayerMask.NameToLayer("OneWayPlatform");
                }
            }
        }

        if (HandleCollisions)
        {
            if (HandlePlatforms)
            {
                handlePlatforms();
            }

            calculateRayOrigins();

            if (deltaMovement.y < 0 && wasGroundedBeforeMoving)
            {
                handleVerticalSlope(ref deltaMovement);
            }

            if (deltaMovement.x != 0)
            {
                collisionState.FaceDirection = (int)Mathf.Sign(deltaMovement.x);
            }


            if (Mathf.Abs(deltaMovement.x) > 0.01f)
            {
               
                moveHorizontally(ref deltaMovement);
            }


            moveVertically(ref deltaMovement);

            // for moving platforms
            correctHorizontalPlacement(ref deltaMovement, true);
            correctHorizontalPlacement(ref deltaMovement, false);

        }

        DeltaMovementX = Mathf.Abs(deltaMovement.x);
        transform.Translate(deltaMovement, Space.World);
        
        if (Time.deltaTime > 0)
        {

            velocity = deltaMovement / Time.deltaTime;
        }

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        if (collisionState.IsMovingUpSlope)
        {
            velocity.y = 0;
        }

        if (StandingOn != null)
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            if (lastStandingOn != StandingOn)
            {
                if (lastStandingOn != null)
                {
                    lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                }

                StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                lastStandingOn = StandingOn;
            }
            else if (StandingOn != null)
            {
                StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (lastStandingOn != null)
        {
            lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            lastStandingOn = null;
        }

    }

    #endregion

    #region Private Movement Methods

    private void handlePlatforms()
    {
        if (StandingOn != null)
        {
            var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(activeLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
            {
                //Debug.Log("moveDistance = " + moveDistance );
                transform.Translate(moveDistance, Space.World);
            }

            PlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
        }
        else
        {
            PlatformVelocity = Vector3.zero;
        }

        StandingOn = null;
    }

    private void calculateRayOrigins()
    {
        var scaledColliderSize = new Vector2(boxCollider.size.x * Mathf.Abs(transform.localScale.x), boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
        var scaledCenter = new Vector2(boxCollider.offset.x * transform.localScale.x, boxCollider.offset.y * transform.localScale.y);

        raycastOrigins.topRight = transform.position + new Vector3(scaledCenter.x + scaledColliderSize.x, scaledCenter.y + scaledColliderSize.y);
        raycastOrigins.topRight.x -= skinWidth;
        raycastOrigins.topRight.y -= skinWidth;

        raycastOrigins.topLeft = transform.position + new Vector3(scaledCenter.x - scaledColliderSize.x, scaledCenter.y + scaledColliderSize.y);
        raycastOrigins.topLeft.x += skinWidth;
        raycastOrigins.topLeft.y -= skinWidth;

        raycastOrigins.bottomRight = transform.position + new Vector3(scaledCenter.x + scaledColliderSize.x, scaledCenter.y - scaledColliderSize.y);
        raycastOrigins.bottomRight.x -= skinWidth;
        raycastOrigins.bottomRight.y += skinWidth;

        raycastOrigins.bottomLeft = transform.position + new Vector3(scaledCenter.x - scaledColliderSize.x, scaledCenter.y - scaledColliderSize.y);
        raycastOrigins.bottomLeft.x += skinWidth;
        raycastOrigins.bottomLeft.y += skinWidth;
    }

    private void handleVerticalSlope(ref Vector2 deltaMovement)
    {

        var center = (raycastOrigins.bottomLeft.x + raycastOrigins.bottomRight.x) * 0.5f;
        var direction = -Vector2.up;

        var slopeDistance = slopeLimitTangent * (raycastOrigins.bottomRight.x - center);
        var slopeRay = new Vector2(center, raycastOrigins.bottomLeft.y);

        DrawRay(slopeRay, direction * slopeDistance, Color.green);
        var mask = PlatformMask;

        mask &= ~OneWayPlatformMask;

        RaycastHit2D raycastHit = Physics2D.Raycast(slopeRay, direction, slopeDistance, PlatformMask); //mask &= ~OneWayPlatformMask;
        if (!raycastHit)
            return;


        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
        if (!isMovingDownSlope)
            return;



        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);

        if (Mathf.Abs(angle) < 0.0001f)
            return;

        collisionState.IsMovingDownSlope = true;
        collisionState.SlopeAngle = angle;
        deltaMovement.y = raycastHit.point.y - slopeRay.y;
    }

    private void moveHorizontally(ref Vector2 deltaMovement)
    {
        float directionX = collisionState.FaceDirection;
        var rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        if (Mathf.Abs(deltaMovement.x) < skinWidth)
        {
            rayDistance = 2 * skinWidth;
        }

        var isGoingRight = deltaMovement.x > 0;

        var rayDirection = Vector2.right * directionX;
        var rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;


        for (var i = 0; i < totalHorizontalRays; i++)
        {
            var ray = new Vector2(rayOrigin.x, rayOrigin.y + i * verticalDistanceBetweenRays);
            DrawRay(ray, rayDirection * rayDistance, Color.green);

            RaycastHit2D raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, PlatformMask & ~OneWayPlatformMask);
            if (!raycastHit)
                continue;

            // the bottom ray can hit slopes but no other ray can so we have special handling for those cases
            if (i == 0 && handleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), directionX == 1 ? true : false)) //isGoingRight
            {
                break;
            }

            // set our new deltaMovement and recalculate the rayDistance taking it into account
            deltaMovement.x = raycastHit.point.x - ray.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (directionX == 1)
            {
                deltaMovement.x -= skinWidth;
                collisionState.Right = true;
            }
            else if (directionX == -1)
            {
                deltaMovement.x += skinWidth;
                collisionState.Left = true;
            }

            

            // we add a small fudge factor for the float operations here. if our rayDistance is smaller
            // than the width + fudge bail out because we have a direct impact
            if (rayDistance < skinWidth + kSkinWidthFloatFudgeFactor)
            {
                break;
            }
        }
    }

    private bool handleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {

        if (Mathf.RoundToInt(angle) == 90)
            return false;

        if (angle > Parameters.SlopeLimit)
        {
            deltaMovement.x = 0;
            return true;
        }

        if (deltaMovement.y > 0.07f)
        {
            return true;
        }


        deltaMovement.x += isGoingRight ? -skinWidth : skinWidth;
        deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
        collisionState.IsMovingUpSlope = true;
        collisionState.Below = true;
        return true;
    }

    private void moveVertically(ref Vector2 deltaMovement)
    {
        //Debug.Log("before deltaMovement.y = " + deltaMovement.y);
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;

        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;

        // apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
        rayOrigin.x += deltaMovement.x;

        // if we are moving up, we should ignore the layers in oneWayPlatformMask
        var mask = PlatformMask;
        if (isGoingUp)
            mask &= ~OneWayPlatformMask;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < totalVerticalRays; i++)
        {

            var ray = new Vector2(rayOrigin.x + i * horizontalDistanceBetweenRays, rayOrigin.y);
            DrawRay(ray, rayDirection * rayDistance, Color.yellow);

            RaycastHit2D raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, mask);
            if (!raycastHit)
                continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = transform.position.y - raycastHit.point.y;

                if (verticalDistanceBetweenRays < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = raycastHit.collider.gameObject;
                }
            }

            // set our new deltaMovement and recalculate the rayDistance taking it into account
            deltaMovement.y = raycastHit.point.y - ray.y;


            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= skinWidth;

                collisionState.Above = true;
            }
            else
            {
                deltaMovement.y += skinWidth;
                collisionState.Below = true;
            }


            if (!isGoingUp && deltaMovement.y > 0.0001f)
            {
                collisionState.IsMovingUpSlope = true;
            }

            // we add a small fudge factor for the float operations here. if our rayDistance is smaller
            // than the width + fudge bail out because we have a direct impact
            if (rayDistance < skinWidth + kSkinWidthFloatFudgeFactor)
                break;

        }
    }

    private void correctHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {

        var halfWidth = (boxCollider.size.x * Mathf.Abs(localScale.x)) * 0.5f;
        var rayOrigin = isRight ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

        if (isRight)
        {
            rayOrigin.x -= (halfWidth - skinWidth);
        }
        else
        {
            rayOrigin.x += (halfWidth - skinWidth);
        }

        //Debug.Log("rayOrigin.x = " + rayOrigin.x);

        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0.0f;

        for (var i = 0; i < totalHorizontalRays - 1; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + deltaMovement.x, rayOrigin.y + deltaMovement.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;

            offset = isRight ? ((raycastHit.point.x - transform.position.x) - halfWidth) : (halfWidth - (transform.position.x - raycastHit.point.x));

        }

        if (Mathf.Abs(offset) >= 0.2f)//0.2f
        {
            //Debug.Log("offset = " + offset);
            deltaMovement.x += offset;
        }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // jump into water or other 
        var parameters = col.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;

        overrideParameters = parameters.Parameters;
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        // jump out water or other
        var parameters = col.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;

        overrideParameters = null;
    }

    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }

    #endregion


}
