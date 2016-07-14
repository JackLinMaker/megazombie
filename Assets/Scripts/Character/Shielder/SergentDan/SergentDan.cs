using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SergentDan : BaseEntity
{
    public GameObject Venom;
    public Transform ShootPoint;
    public GameObject ArmSkeleton;
    public bool IsInAttackAction;
    public GameObject BossCRAttackLight;
    public GameObject EarthQuakeEffect;
    public float CRDamage;
    public float EarthQuakeDamage;
    public LayerMask PlayerLayerMask;
    public List<SergentDanChargeState.ChargeState> AttackCommand;

    private float Vy;
    private float Duration;
    private float Gravity = 20f;
    private float Vx;
    private float MaxHealth;
    private bool isDizzy;

    private StateMachine<SergentDan> stateMachine;

    public override void Awake()
    {
        base.Awake();
        Target = Player;
        MaxHealth = CurrentHealth;
        isDizzy = false;
        IsInAttackAction = false;
        AttackCommand = new List<SergentDanChargeState.ChargeState>();
        AttackCommand.Add(SergentDanChargeState.ChargeState.Chase);
        AttackCommand.Add(SergentDanChargeState.ChargeState.Chase);
        AttackCommand.Add(SergentDanChargeState.ChargeState.Chase);
        AttackCommand.Add(SergentDanChargeState.ChargeState.SpecialAttack);
        AttackCommand.Add(SergentDanChargeState.ChargeState.SpecialAttack);
        stateMachine = new StateMachine<SergentDan>(this);
        stateMachine.ChangeState(SergentDanIdleState.Instance);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<SergentDan> GetFSM()
    {
        return stateMachine;
    }

    public void CRAttack()
    {
        GameObject CREffect = Instantiate(BossCRAttackLight, transform.position + new Vector3(isFacingRight ? 0.6f : -0.6f, 1f, 0), Quaternion.Euler(new Vector3(0, isFacingRight ? 0 : -180, 0))) as GameObject;

        Vector2 start = new Vector2(transform.position.x, controller.raycastOrigins.bottomLeft.y + boxCollider.bounds.size.y * 0.5f);
        Vector2 end = start + new Vector2(isFacingRight ? 2 : -2, 0);
        Debug.DrawLine(start, end, Color.red);
        RaycastHit2D[] casts = Physics2D.LinecastAll(start, end, PlayerLayerMask);
        // 与敌人的碰撞器进行相交检测
        for (int i = 0; i < casts.Length; i++)
        {
            if (casts[i].collider.gameObject.GetComponent<Player>())
            {
                if (casts[i].collider.transform.GetComponent<Player>().CurrentHealth > 0)
                {
                    casts[i].collider.transform.GetComponent<Player>().Hurt(CRDamage, isFacingRight);
                }
            }
        }
    }

    public void FinishCRAttack()
    {
        IsInAttackAction = false;
    }

    public void EarthQuake()
    {
        IsInAttackAction = false;
        this.Animator.SetBool("Jump", false);

        Director.Instance.ShakeScreen();

        GameObject EarthQuake = Instantiate(EarthQuakeEffect, transform.position + new Vector3(0, 2.8f, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

        Vector2 start = new Vector2(transform.position.x, controller.raycastOrigins.bottomLeft.y + boxCollider.bounds.size.y * 0.5f) + new Vector2(3, 0);
        Vector2 end = new Vector2(transform.position.x, controller.raycastOrigins.bottomLeft.y + boxCollider.bounds.size.y * 0.5f) + new Vector2(-3, 0);
        Debug.DrawLine(start, end, Color.red);
        RaycastHit2D[] casts = Physics2D.LinecastAll(start, end, PlayerLayerMask);
        // 与敌人的碰撞器进行相交检测
        for (int i = 0; i < casts.Length; i++)
        {
            if (casts[i].collider.gameObject.GetComponent<Player>())
            {
                if (casts[i].collider.transform.GetComponent<Player>().CurrentHealth > 0)
                {
                    casts[i].collider.transform.GetComponent<Player>().Hurt(EarthQuakeDamage, true);
                }
            }
        }

    }

    public void LRAttack()
    {
        /*GameObject venom = Instantiate(Venom, ShootPoint.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        venom.transform.GetComponent<Venom>().TargetPosition = Player.position;
        venom.transform.GetComponent<Venom>().StartMove(true);*/
    }

    public void Fly()
    {
        Vy = 10;
        float distanceX;
        distanceX = Player.position.x - transform.position.x;
        float distanceY = Player.position.y - transform.position.y;
        float time_to_top = Vy / Gravity;
        float jump_height = Vy * time_to_top - Gravity * time_to_top * time_to_top * 0.5f;
        float falling_height = jump_height - distanceY;
        float time_to_fall = Mathf.Sqrt(2 * falling_height / Gravity);
        float total_time = time_to_top + time_to_fall;
        Duration = total_time;

        Vx = distanceX / total_time;

        StartCoroutine(flyCo());
    }

    private IEnumerator flyCo()
    {
        float elapsed = 0.0f;
        while (elapsed < Duration)
        {

            this.Controller.SetForce(new Vector3(Vx, (Vy - (Gravity * elapsed)), 0.0f));
            elapsed += Time.deltaTime;
            yield return null;

        }
        this.Controller.SetHorizontalForce(0.0f);
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        //SoundManager.Instance.MakeHitZombieSound(this.transform.position);
        CurrentHealth -= damage;

        // 受伤变红
        StartCoroutine("HurtColorChange");

        if (CurrentHealth <= 0)
        {
          
            boxCollider.enabled = false;
            stateMachine.ChangeState(SergentDanDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
          
            addHurtEffect(pos, direction > 0 ? true : false);

            if ((CurrentHealth / MaxHealth) < 0.5f && !isDizzy)
            {
                isDizzy = true;

                stateMachine.ChangeState(SergentDanDizzyState.Instance);
            }
            controller.MoveBack(direction, 5.0f, 0.1f);

        }


    }

    public override void Dead()
    {
        Vector3 pos = this.transform.position + new Vector3(0, 0.7f, 0);
        addDeadEffect(pos);
        StartCoroutine(BodyCo());
    }

    private IEnumerator BodyCo()
    {
        yield return new WaitForSeconds(0.01f);
        Animator.SetBool("Dead", true);
        Disapper();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.gameObject.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER && CurrentHealth > 0)
            {
                Vector3 pos = bullet.transform.position;
                bullet.DestoryWithEffect();
                Hurt(bullet.Damage, pos, bullet.IsFacingRight ? 1 : -1);
                //HitBack(bullet.IsFacingRight == true ? bullet.HitForceX * 0.5f : -bullet.HitForceX * 0.5f, bullet.HitForceY, 0.1f);
            }
        }
    }

}
