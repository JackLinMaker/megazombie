using UnityEngine;
using System.Collections;

public class Bomber : BaseEntity
{
    public Rigidbody2D Bomb;
    public Transform SpawnPoint;
    public GameObject Bomb_Mo;
    public int AttackStateHash;
    public float Direction;
    public Vector2 BombVelocity;

    private StateMachine<Bomber> stateMachine;
    private Vector3 targetPosition;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        AttackStateHash = Animator.StringToHash("Base Layer.Attack");
        Flip(Direction);
        stateMachine = new StateMachine<Bomber>(this);

        if (PatrolPath != null)
        {
            stateMachine.ChangeState(BomberPatrolState.Instance);
        }
        else
        {
            stateMachine.ChangeState(BomberIdleState.Instance);
        }


    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<Bomber> GetFSM()
    {
        return stateMachine;
    }

    public override void TriggerAttackEvent()
    {

        if (!isAttacking)
        {
            Animator.SetBool("Attacking", true);
            isAttacking = true;
        }

    }

    public override void Attack()
    {
        if (Target != null)
        {
            Bomb_Mo.SetActive(false);
            if (isFacingRight == true)
            {
                Rigidbody2D bomb = Instantiate(Bomb, SpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bomb.transform.GetComponent<Bomb>().Target = targetPosition;
                bomb.transform.GetComponent<Bomb>().Damage = Damage;
                bomb.velocity = BombVelocity;
              
            }
            else
            {
                Rigidbody2D bomb = Instantiate(Bomb, SpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bomb.transform.GetComponent<Bomb>().Target = targetPosition;
                bomb.transform.GetComponent<Bomb>().Damage = Damage;
                bomb.velocity = new Vector2(-BombVelocity.x, BombVelocity.y);
                

            }


        }
        else
        {
            ResetAttackEvent();
        }

    }

    public override void ResetAttackEvent()
    {
        Bomb_Mo.SetActive(true);
        isAttacking = false;
        Animator.SetBool("Attacking", false);
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0) return;
        SoundManager.Instance.MakeHitZombieSound(this.transform.position);
        CurrentHealth -= damage;

        // 受伤变红
        StartCoroutine("HurtColorChange");

        if (CurrentHealth <= 0)
        {

            deadDirection = direction;
            stateMachine.ChangeState(BomberDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            addHurtEffect(pos, direction > 0 ? true : false);

            if (stateMachine.IsInState(BomberPatrolState.Instance) || stateMachine.IsInState(BomberPauseState.Instance) || stateMachine.IsInState(BomberIdleState.Instance))
            {
                perspective.Detected = true;
                stateMachine.ChangeState(BomberAttackState.Instance);
            }

           

        }
    }

  
    public virtual void Dead()
    {
        Director.Instance.ShakeScreen();
        perspective.enabled = false;
        addDeadEffect(boxCollider.bounds.center);
        Animator.SetBool("Dead", true);
        HitBack(deadDirection >= 0 ? 2.5f : -2.5f, 5.0f, 0.1f);
        Disapper();
    }

    public override void OnFire()
    {
        base.OnFire();
        AnimatorStateInfo asi = Animator.GetCurrentAnimatorStateInfo(0);
        /*if (!asi.IsName("Base Layer.TransAttack"))
        {
            attacktime += Time.deltaTime;
        }*/

        if (CurrentHealth > 0 && !this.GetFSM().IsInState(BomberFleeState.Instance))
        {
            this.GetFSM().ChangeState(BomberFleeState.Instance);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.gameObject.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER)
            {
                Target = bullet.Host;
                if (CurrentHealth > 0)
                {
                    Vector3 pos = bullet.transform.position;
                    bullet.DestoryWithEffect();
                    Hurt(bullet.Damage, pos, bullet.IsFacingRight ? 1 : -1);

                    //HitBack(bullet.IsFacingRight == true ? bullet.HitForceX : -bullet.HitForceX, bullet.HitForceY, 0.1f);

                }



            }
        }
    }
}
