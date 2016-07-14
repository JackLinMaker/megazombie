using UnityEngine;
using System.Collections;

public class Destroyer : BaseEntity
{
    public int Direction;

    public float DamageRadius = 3.0f;

    public float ExplosionDamage = 60;

    public Collider2D[] BodyPiece;

    private StateMachine<Destroyer> stateMachine;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Destroyer>(this);

        if (PatrolPath != null)
        {
            stateMachine.ChangeState(DestroyerPatrolState.Instance);
        }
        else
        {
            stateMachine.ChangeState(DestroyerIdleState.Instance);
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<Destroyer> GetFSM()
    {
        return stateMachine;
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0)
            return;
        SoundManager.Instance.MakeHitZombieSound(this.transform.position);
        CurrentHealth -= damage;

        // 受伤变红
        StartCoroutine("HurtColorChange");

        if (CurrentHealth <= 0)
        {
            deadDirection = direction;

            stateMachine.ChangeState(DestroyerDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            addHurtEffect(pos, direction > 0 ? true : false);
            if (stateMachine.IsInState(DestroyerPatrolState.Instance) || stateMachine.IsInState(DestroyerPauseState.Instance) || stateMachine.IsInState(DestroyerIdleState.Instance))
            {
                if (perspective.enabled == true)
                {
                    perspective.Detected = true;

                    stateMachine.ChangeState(DestroyerChaseState.Instance);

                }

            }
        }

    }

    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }

    public override void Dead()
    {
        this.transform.gameObject.SetActive(false);
        Director.Instance.ShakeScreen();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + boxCollider.bounds.size.y * 0.5f, transform.position.z);
        for (int i = 0; i < BodyPiece.Length; i++)
        {
            Collider2D piece = Instantiate(BodyPiece[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as Collider2D;
            piece.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(3.0f,10.0f));
        }
        if (DeadEffect)
        {
            GameObject Effect = Instantiate(DeadEffect, transform.position, Quaternion.identity) as GameObject;
            Collider2D[] casts = Physics2D.OverlapCircleAll(this.transform.position, DamageRadius);
            for (int i = 0; i < casts.Length; i++)
            {

                if (casts[i].transform.GetComponent<Player>() != null)
                {
                    Player player = casts[i].transform.GetComponent<Player>();
                    player.Hurt(ExplosionDamage, isFacingRight);
                    float dis = transform.position.x - player.transform.position.x;
                    player.HitBack(dis <= 0 ? 5.0f : -5.0f, 5.0f, 0.25f);
                }
                else if (casts[i].transform.GetComponent<InteractiveItem>() != null)
                {
                    InteractiveItem item = casts[i].transform.GetComponent<InteractiveItem>();
                    item.Hurt(ExplosionDamage);
                }
                else if (casts[i].transform.GetComponent<BaseEntity>() != null && this.transform != casts[i].transform)
                {
                    BaseEntity entity = casts[i].transform.GetComponent<BaseEntity>();
                    entity.Hurt(ExplosionDamage, entity.transform.position, 0);
                }
            }
        }

        Destroy(gameObject);


    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.gameObject.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER /*|| bullet.Owner == Util.ObjectOwner.Soldier*/)
            {
                Target = bullet.Host;
                if (CurrentHealth > 0)
                {
                    Vector3 pos = bullet.transform.position;
                    bullet.DestoryWithEffect();
                    Hurt(bullet.Damage, pos, bullet.IsFacingRight ? 1 : -1);


                }
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Controller.SetHorizontalForce(0.0f);
            stateMachine.ChangeState(DestroyerDeadState.Instance);
        }
        else if (collider.gameObject.tag == "Bounds")
        {
            Controller.SetHorizontalForce(0.0f);
            stateMachine.ChangeState(DestroyerDeadState.Instance);
        }
    }

    public override void OnFire()
    {
        base.OnFire();
    }

}
