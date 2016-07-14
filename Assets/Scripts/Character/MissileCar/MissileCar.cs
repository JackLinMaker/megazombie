using UnityEngine;
using System.Collections;

public class MissileCar : BaseEntity
{
    public GameObject Bullet;
    public float BulletSpeed;
    public float HitForceX;
    public float HitForceY;
    public float Range;
    public Transform ShootPointLeft1;
    public Transform ShootPointLeft2;
    public Transform ShootPointRight1;
    public Transform ShootPointRight2;
    public GameObject FrontShootEffect;
    public GameObject BackgroundShootEffect;
    public Collider2D Blood;
    private StateMachine<MissileCar> stateMachine;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<MissileCar>(this);
        stateMachine.ChangeState(MissileCarIdleState.Instance);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<MissileCar> GetFSM()
    {
        return stateMachine;
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        
        CurrentHealth -= damage;
        StartCoroutine("HurtColorChange");

        if (CurrentHealth <= 0)
        {
            deadDirection = direction;
            Dead();
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
    }

    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }

    public override void Dead()
    {
        perspective.enabled = false;
        addDeadEffect(boxCollider.bounds.center);
        Destroy(gameObject);
    }

    protected void addDeadEffect(Vector3 pos)
    {
        if (Contents.Length > 0)
        {
            for (int i = 0; i < Contents.Length; i++)
            {
                GameObject item = Instantiate(Contents[i], boxCollider.bounds.center, Quaternion.identity) as GameObject;
                item.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 5f));
            }
        }

        if (DeadEffect != null)
        {
            Quaternion r1 = Quaternion.Euler(-90.0f, -90.0f, 0.0f);
            GameObject Effect = Instantiate(DeadEffect, pos, r1) as GameObject;
        }

    }

    public void AttackLeft1()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointLeft1);
    }

    public void AttackLeft2()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointLeft2);
    }

    public void AttackRight1()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointRight1);
    }

    public void AttackRight2()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointRight2);
    }

    private void ShootBullet(Transform host, bool isFacingRight, Util.ObjectOwner owner, Transform shootPoint)
    {
        Vector3 pos = new Vector3(shootPoint.position.x, shootPoint.position.y, 0.0f);
        Debug.Log("pos = " + pos);

        GameObject bulletInstance = Instantiate(Bullet, pos, Quaternion.identity) as GameObject;
        bulletInstance.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
        BaseBullet bullet = bulletInstance.GetComponent<BaseBullet>();
        if (bullet != null)
        {
            ((Missile)bullet).Target = Player;
            bullet.Speed = BulletSpeed;
            bullet.Damage = Damage;
            bullet.HitForceX = HitForceX;
            bullet.HitForceY = HitForceY;
            bullet.IsFacingRight = isFacingRight;
            bullet.Range = Range;
            bullet.Owner = owner;
            bullet.Host = host;
            bullet.Direction = Quaternion.Euler(0, 0, Random.Range(-5.0f, 5.0f)) * Vector3.right;
        }

        addShootEffect(isFacingRight, this.transform, shootPoint);
    }

    protected void addShootEffect(bool isFacingRight, Transform host, Transform shoot)
    {
        if (FrontShootEffect != null)
        {
            GameObject shootEffect = Instantiate(FrontShootEffect, shoot.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
            shootEffect.transform.parent = host;
        }

        if (BackgroundShootEffect != null)
        {
            GameObject bshootEffect = Instantiate(BackgroundShootEffect, shoot.position + new Vector3(isFacingRight ? -1f : 1f, 0, 0), Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
            bshootEffect.transform.parent = host;
        }
    }

    public void TriggerAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Animator.SetBool("Attacking", true);
        }
    }

    public override void ResetAttackEvent()
    {
        isAttacking = false;
        Animator.SetBool("Attacking", false);
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

    }

    public override void OnFire()
    {
        base.OnFire();
    }
}
