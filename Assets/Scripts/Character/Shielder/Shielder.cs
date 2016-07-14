using UnityEngine;
using System.Collections;

public class Shielder : BaseEntity
{

    public Transform Shield;
    public Collider2D Piece;
    public GameObject Bullet;
    public float BulletSpeed;
    public float HitForceX;
    public float HitForceY;
    public float Range;
    public Transform ShootPoint;
    public Rigidbody2D ShellCase;
    public GameObject ShootEffect;
    public float Direction;
    public GameObject Shield_Mo;

    private StateMachine<Shielder> stateMachine;

    #region SystemFunc

    public override void Awake()
    {
        base.Awake();
        Flip(Direction);
        stateMachine = new StateMachine<Shielder>(this);
        if (PatrolPath != null)
        {
            stateMachine.ChangeState(ShielderPatrolState.Instance);
        }
        else
        {
            stateMachine.ChangeState(ShielderIdleState.Instance);
        }

    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();

    }

    #endregion

    public StateMachine<Shielder> GetFSM()
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

    public void TriggerAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Animator.SetBool("Attack", true);
        }
    }

    public void FastAttack()
    {
        //if (!isAttacking)
        //{
        //    isAttacking = true;
        this.Animator.Play("Base Layer.Attacking", 0, 0);
        //}
    }

    public override void ResetAttackEvent()
    {
        isAttacking = false;
        Animator.SetBool("Attack", false);
    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0) return;
        if (Target.GetComponent<Player>().isFacingRight != isFacingRight && Shield.GetComponent<Collider2D>().enabled == true)
        {
            Shield.GetComponent<Shield>().Hurt(damage);
            return;
        }

        SoundManager.Instance.MakeHitBodySound(this.transform.position);
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {

            deadDirection = direction;
            stateMachine.ChangeState(ShielderDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {

            // 受伤变红
            StartCoroutine("HurtColorChange");


            addHurtEffect(pos, direction > 0 ? true : false);
            //HitBack(direction >= 0 ? 10.0f : -10.0f);

            if (stateMachine.IsInState(ShielderPatrolState.Instance) || stateMachine.IsInState(ShielderPauseState.Instance) || stateMachine.IsInState(ShielderIdleState.Instance))
            {
                float distance = Mathf.Abs(Player.position.x - transform.position.x);
                stateMachine.ChangeState(ShielderDefenseState.Instance);
            }

           
        }

    }

    public void ChangeDeadColor()
    {
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials[0].shader = Shader.Find("Sprites/Gray");
        }
    }

    public override void Dead()
    {
        Director.Instance.ShakeScreen();
        perspective.enabled = false;
        addDeadEffect(boxCollider.bounds.center);
        Animator.SetBool("Dead", true);
        HitBack(deadDirection >= 0 ? 2.5f : -2.5f, 5.0f, 0.1f);
        Shield_Mo.SetActive(false);
        Collider2D piece = Instantiate(Piece, transform.position + new Vector3(0.9f, 0.5f, 0), Quaternion.identity) as Collider2D;
        piece.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3, 5) * deadDirection, Random.Range(4, 6));
        Disapper();
    }

    public void ShowShield()
    {
        if (Shield.GetComponent<Collider2D>())
        {
            Shield.GetComponent<Collider2D>().enabled = true;
        }

    }

    public void HideShield()
    {
        if (Shield.GetComponent<Collider2D>())
        {
            Shield.GetComponent<Collider2D>().enabled = false;
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

                }

            }

        }
    }

    public override void OnFire()
    {
        base.OnFire();

    }


}
