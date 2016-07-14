using UnityEngine;
using System.Collections;

public class Soldier : BaseEntity
{
    public GameObject Bullet;
    public float BulletSpeed;
    public float HitForceX;
    public float HitForceY;
    public float Range;
    public Transform ShootPoint;
    public Rigidbody2D ShellCase;
    public GameObject ShootEffect;
    public Collider2D Blood;

   

    private StateMachine<Soldier> stateMachine;

    #region SystemFunc

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Soldier>(this);

        if (PatrolPath != null)
        {
            stateMachine.ChangeState(SoldierPatrollState.Instance);
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();


    }

    #endregion

    public StateMachine<Soldier> GetFSM()
    {
        return stateMachine;
    }

    public override void Attack()
    {
        ShootBullet(this.transform, isFacingRight, Util.ObjectOwner.Soldier);
    }

    private void ShootBullet(Transform host, bool isFacingRight, Util.ObjectOwner owner)
    {
        GameObject bulletInstance = Instantiate(Bullet, ShootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0.0f : 180.0f))) as GameObject;
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


        addShootEffect(isFacingRight, this.transform);
        addShellCase(isFacingRight);
    }

    protected void addShootEffect(bool isFacingRight, Transform host)
    {
        if (ShootEffect != null)
        {
            GameObject shootEffect = Instantiate(ShootEffect, ShootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
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

    protected void addShellCase(bool isFacingRight)
    {
        if (ShellCase != null)
        {
            Vector3 shellPos = ShootPoint.position;
            Rigidbody2D shellCase = Instantiate(ShellCase, shellPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0.0f, 180.0f)))) as Rigidbody2D;
            shellCase.velocity = new Vector2(isFacingRight ? Random.Range(-4.0f, -2.0f) : Random.Range(2.0f, 4.0f), Random.Range(2.0f, 4.0f));
            shellCase.angularVelocity = 360.0f;
        }

    }

    public override void TriggerAttackEvent()
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

    public override void ResetAttackEvent()
    {
        isAttacking = false;
        Animator.SetBool("Attacking", false);


    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0) return;
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
        CurrentHealth -= damage;

        // 受伤变红
        StartCoroutine("HurtColorChange");

        if (CurrentHealth <= 0)
        {
            deadDirection = direction;
            stateMachine.ChangeState(SoldierDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            // add hurt effect
            addHurtEffect(pos, direction > 0 ? true : false);

            if (stateMachine.IsInState(SoldierPatrollState.Instance) || stateMachine.IsInState(SoldierPauseState.Instance))
            {
                perspective.Detected = true;
                stateMachine.ChangeState(SoldierChaseState.Instance);
            }

            

        }

    }

 
    public override void Dead()
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
                    
                }


            }

        }
    }



}
