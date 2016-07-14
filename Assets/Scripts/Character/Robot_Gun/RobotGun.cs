using UnityEngine;
using System.Collections;

public class RobotGun : BaseEntity
{
    public GameObject Bullet;
    public float BulletSpeed;
    public float HitForceX;
    public float HitForceY;
    public float Range;
    public Transform ShootPointLeft;
    public Transform ShootPointRight;
    public Rigidbody2D ShellCase;
    public GameObject ShootEffect;

    private StateMachine<RobotGun> stateMachine;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<RobotGun>(this);

        if (PatrolPath != null)
        {
            stateMachine.ChangeState(RobotGunIdleState.Instance);
        }
        else
        {
            stateMachine.ChangeState(RobotGunIdleState.Instance);
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<RobotGun> GetFSM()
    {
        return stateMachine;
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damage;

        // 受伤变红
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

    public void AttackLeft()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointLeft);
    }

    public void AttackRight()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier, ShootPointRight);
    }

    private void ShootBullet(Transform host, bool isFacingRight, Util.ObjectOwner owner, Transform shootPoint)
    {
        GameObject bulletInstance = Instantiate(Bullet, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0.0f : 180.0f))) as GameObject;
        BaseBullet bullet = bulletInstance.GetComponent<BaseBullet>();
        if (bullet != null)
        {
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
        //addShellCase(isFacingRight, shootPoint);
    }

    private IEnumerator lineRendererCo(Transform shootPoint)
    {
        yield return new WaitForSeconds(0.05f);
        shootPoint.GetComponent<LineRenderer>().SetVertexCount(0);
    }

    protected void addShootEffect(bool isFacingRight, Transform host, Transform shoot)
    {
        if (ShootEffect != null)
        {
            GameObject shootEffect = Instantiate(ShootEffect, shoot.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
            if (isFacingRight)
            {
                shootEffect.transform.FindChild("GunLight01").GetComponent<ParticleSystem>().startRotation = 0;
            }
            else
            {
                // 弧度
                shootEffect.transform.FindChild("GunLight01").GetComponent<ParticleSystem>().startRotation = 3.1415927f;
            }
            shootEffect.transform.parent = host;
        }
    }

    protected void addShellCase(bool isFacingRight, Transform shoot)
    {
        if (ShellCase != null)
        {
            Vector3 shellPos = shoot.position;
            Rigidbody2D shellCase = Instantiate(ShellCase, shellPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0.0f, 180.0f)))) as Rigidbody2D;
            shellCase.velocity = new Vector2(isFacingRight ? Random.Range(-4.0f, -2.0f) : Random.Range(2.0f, 4.0f), Random.Range(2.0f, 4.0f));
            shellCase.angularVelocity = 360.0f;
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
