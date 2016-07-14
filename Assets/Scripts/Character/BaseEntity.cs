using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class BaseEntity : MonoBehaviour
{
    public enum BodyPart
    {
        HEAD,
        BODY,
    }

    public enum EnemyType
    {
        Zombie,
        Soldier,
        SoundSeeker,
        Crasher,
        Shielder,
        Bomber,
        Destroyer,
        HeavyMachiner,
        MissleCar,
        RobotGun,
        Boss1,
        Bat,
    }

   


    public EnemyType Type;

    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public PathDefinition PatrolPath;
    public IEnumerator<Transform> currentPoint;
    [HideInInspector]
    public Transform Player;
    public Animator Animator;
    public float CurrentHealth;
    public float PauseTime = 2.0f;
    public float ChaseRange = 5.0f;
    public float AttackRange = 2.0f;
    public float ChaseFactor = 1.0f;
    public float Damage;
    public GameObject[] Contents;
    public SkinnedMeshRenderer[] MeshRenderers;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public float AttackDuration = 0.0f;

    public bool IsHitBack { get; set; }
    public GameObject HurtEffect;
    public GameObject DeadEffect;
    public GameObject OnFireEffect;


    protected Rigidbody2D rigidbody;
    protected BoxCollider2D boxCollider;
    protected Perspective perspective;
    protected CharacterController2D controller;

    public bool isFacingRight = true;
    protected float normalizedHorizontalSpeed;
    protected float pauseTimer = 0.0f;
    protected bool isAttacking = false;
    protected Vector3 StartPos;
    protected Vector3 EndPos;
    protected int deadDirection;
    protected Transform Question;
    protected Transform Exclamation;



    public CharacterController2D Controller
    {
        get
        {
            return controller;
        }
    }

    public Perspective Perspective
    {
        get
        {
            return perspective;
        }
    }


    public virtual void Awake()
    {
      
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        controller = GetComponent<CharacterController2D>();
        perspective = this.transform.GetComponent<Perspective>();
     
        if (PatrolPath != null)
        {
            currentPoint = PatrolPath.GetPathEnumerator();
            currentPoint.MoveNext();
            StartPos = PatrolPath.Points[0].position;
            EndPos = PatrolPath.Points[1].position;
        }


        isFacingRight = Animator.transform.localScale.x > 0 ? true : false;
        Question = transform.FindChild("Question");
        Exclamation = transform.FindChild("Exclamation");
    }
    
    void OnDestroy()
    {
        if (GameManager.instance && GameManager.instance.gameObject.activeInHierarchy)
        {
            GameManager.instance.Enemys.Remove(this);
        }

    }

   
    public virtual void Update()
    {
        this.Animator.speed = 1.0f;
      
        if (CurrentHealth < 0 && controller.IsGrounded)
        {
            controller.SetHorizontalForce(0.0f);
        }
    }

    public void Patrol()
    {
        if (currentPoint == null || currentPoint.Current == null)
            return;
       
        moveToDestination(currentPoint.Current);

    }

    public void Spawn(float direction)
    {
        Move(direction);
        StartCoroutine(spawnCo());
    }

    private IEnumerator spawnCo()
    {
        yield return new WaitForSeconds(1.5f);
        controller.SetHorizontalForce(0.0f);
    }

    public void moveToDestination(Transform target)
    {
        if (!target) 
            return;
        float distance = (target.position - transform.position).x;
        
        Move(distance);
    }

   
    private void Move(float direction)
    {
        if (direction >= 0)
        {

            normalizedHorizontalSpeed = 1;
        }
        else if (direction < 0)
        {

            normalizedHorizontalSpeed = -1;
        }

        Flip(normalizedHorizontalSpeed);
        //var movementFactor = controller.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        var movementFactor = controller.IsGrounded ? controller.Parameters.MaxVelocity.x : controller.Parameters.MaxVelocity.x * 0.9f;
        float force = normalizedHorizontalSpeed * movementFactor * ChaseFactor;
        controller.SetHorizontalForce(force);
    }

    public void FaceToTarget()
    {
        if (Target == null)
            return;

        float distance = (Target.position - transform.position).x;

        if (distance >= 0)
        {

            normalizedHorizontalSpeed = 1;
        }
        else if (distance < 0)
        {
            normalizedHorizontalSpeed = -1;
        }

        Flip(normalizedHorizontalSpeed);
    }

    public void Chase()
    {
        if (Target == null)
        {
            return;
        }
        if (!IsHitBack)
        {
            float distance = (Target.position - transform.position).x;
            Move(distance);
        }
        
    }

    public void Flee(float sign)
    {
        

        if (sign >= 0)
        {
            normalizedHorizontalSpeed = 1;
        }
        else if (sign < 0)
        {
            normalizedHorizontalSpeed = -1;
        }

        Flip(normalizedHorizontalSpeed);

        var movementFactor = controller.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        movementFactor *= 0.5f;


      

        float force = normalizedHorizontalSpeed * movementFactor;
        controller.SetHorizontalForce(force);
    }

    public bool Flip(float dx)
    {
        Vector3 scale = this.Animator.transform.localScale;

        if (Mathf.Sign(dx) != Mathf.Sign(scale.x))
        {
            isFacingRight = scale.x > 0 ? false : true;
            this.Animator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            return true;
        }


        return false;
    }

    public bool ReachDestination(Transform target)
    {
        if (!target) 
            return false;
        float distance = Mathf.Abs(target.position.x - transform.position.x);
        if (distance < 0.1f)
        {

            controller.SetHorizontalForce(0.0f);
            return true;
        }

        return false;
    }

    public bool Pause()
    {
        pauseTimer += Time.deltaTime;
        if (pauseTimer >= PauseTime)
        {
            pauseTimer = 0;
            currentPoint.MoveNext();
            return true;
        }

        return false;
    }

    public virtual void TriggerAttackEvent()
    {

        if (!isAttacking)
        {


            float distance = (Target.position - transform.position).x;

            if (distance >= 0)
            {
                normalizedHorizontalSpeed = 1;
            }
            else if (distance < 0)
            {
                normalizedHorizontalSpeed = -1;
            }

            Flip(normalizedHorizontalSpeed);

            Animator.SetBool("Attacking", true);
            isAttacking = true;
        }
    }

    public virtual void ResetAttackEvent()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void Hurt(float damage, Vector3 pos, int direction)
    {

    }

    public virtual void Dead()
    {

    }

    public virtual void Disapper()
    {
        StartCoroutine(DisapperCo());
    }

    private IEnumerator DisapperCo()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetColor("_Color", new Color(1, 1, 1, 0.8f));
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetColor("_Color", new Color(1, 1, 1, 0.6f));
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetColor("_Color", new Color(1, 1, 1, 0.4f));
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetColor("_Color", new Color(1, 1, 1, 0.2f));
        }
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject);
    }

    public virtual void HitBack(float xForce, float yForce, float time)
    {
        IsHitBack = true;
        controller.SetForce(new Vector2(xForce, yForce));
        
        StartCoroutine(stopHitBack(time));
    }

    private IEnumerator stopHitBack(float time)
    {
        yield return new WaitForSeconds(time);
        IsHitBack = false;
        controller.SetHorizontalForce(0.0f);
       
    }

    public void addHurtEffect(Vector3 pos, bool isFacingRight)
    {
        if (HurtEffect != null)
        {
            // Create a quaternion with a random rotation in the z-axis.
            Quaternion randomRotation = Quaternion.Euler(0f, isFacingRight == true ? -180f : 0f, 0f);
            GameObject Effect = Instantiate(HurtEffect, pos, randomRotation) as GameObject;
            Effect.transform.parent = this.transform;
        }
     
    }

    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {

        Debug.DrawRay(start, dir, color);
    }

    protected void addDeadEffect(Vector3 pos)
    {
        if (Contents.Length > 0)
        {
            for (int i = 0; i < Contents.Length; i++)
            {
                Debug.Log("Gold");
                GameObject item = Instantiate(Contents[i], boxCollider.bounds.center, Quaternion.identity) as GameObject;
                item.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7.0f, 7.0f), Random.Range(8.0f, 12.0f));
            }
        }

        if (DeadEffect != null)
        {
            GameObject Effect = Instantiate(DeadEffect, pos, Quaternion.identity) as GameObject;
        }

    }

    public void addDeadEffectDebug()
    {
        if (DeadEffect != null)
        {
            Quaternion r1 = Quaternion.Euler(-90.0f, -90.0f, 0.0f);
            GameObject Effect = Instantiate(DeadEffect, boxCollider.bounds.center, r1) as GameObject;
            Effect.transform.parent = this.transform;
            Effect.transform.localScale = new Vector3(1, 1, 1);
            Effect.GetComponent<ParticleSystem>().startSize = Effect.GetComponent<ParticleSystem>().startSize / 5;
            ParticleSystem[] p = Effect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].startSize = p[i].startSize / 5;
            }
        }

    }

    public void StartSlowMotion()
    {

        Time.timeScale = 0.3f;
        StartCoroutine(slowMotionCo());
    }

    protected IEnumerator slowMotionCo()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    protected IEnumerator HurtColorChange()
    {

        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.5f);
            MeshRenderers[i].materials[0].SetColor("_FlashColor", Color.red);
        }
        yield return new WaitForSeconds(0.08f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.3f);
        }
        yield return new WaitForSeconds(0.03f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0.15f);
        }
        yield return new WaitForSeconds(0.03f);
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].SetFloat("_FlashAmount", 0f);
        }
    }

    public virtual void OnFire()
    {
        
        GameObject fire = Instantiate(OnFireEffect, boxCollider.bounds.center, Quaternion.identity) as GameObject;
        fire.transform.parent = this.transform;
        Destroy(fire, 3.0f);
    }

    public void ShowQuestion()
    {

        StartCoroutine(ShowQuestionCo());

    }

    private IEnumerator ShowQuestionCo()
    {
        Question.gameObject.SetActive(true);
        Question.transform.GetComponent<TweenScale>().PlayForward();
        yield return new WaitForSeconds(1.0f);
        Question.transform.GetComponent<TweenScale>().PlayReverse();
        yield return new WaitForSeconds(0.1f);
        Question.gameObject.SetActive(false);
    }

    public void ShowExclamation()
    {
        StartCoroutine(ShowExclamationCo());
    }

    private IEnumerator ShowExclamationCo()
    {
        Exclamation.gameObject.SetActive(true);
        Exclamation.transform.GetComponent<TweenScale>().PlayForward();
        yield return new WaitForSeconds(1.0f);
        Exclamation.transform.GetComponent<TweenScale>().PlayReverse();
        yield return new WaitForSeconds(0.1f);
        Exclamation.gameObject.SetActive(false);
    }

   

}
