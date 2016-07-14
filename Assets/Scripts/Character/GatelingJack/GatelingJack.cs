using UnityEngine;
using System.Collections;

public class GatelingJack : BaseEntity
{
    public Transform ShootPoint;
    public GameObject ShootEffect;
    public Rigidbody2D ShellCase;
    public GameObject Bullet;
    public float BulletSpeed;
    public float Range;
    
    

    public GameObject Helper;
    public Transform[] SpawnPoints;
    private int HelperCount;
    private int HelperIndex;
    private bool HelperState;

    private float HitForceX;
    private float HitForceY;
    private float MaxHealth;

    private StateMachine<GatelingJack> stateMachine;
    

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<GatelingJack>(this);
        stateMachine.ChangeState(GatelingJackSleepState.Instance);
        HitForceX = 5.0f;
        HitForceY = 0.0f;
        MaxHealth = CurrentHealth;
        HelperIndex = 0;
        HelperState = false;
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<GatelingJack> GetFSM()
    {
        return stateMachine;
    }

    public override void TriggerAttackEvent()
    {

        /*if (!isAttacking)
        {

            
            isAttacking = true;
        }*/
        Attack();

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

    private void addShootEffect(bool isFacingRight, Transform host)
    {
        if (ShootEffect != null)
        {
            GameObject shootEffect = Instantiate(ShootEffect, ShootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
            shootEffect.transform.parent = host;
        }

    }

    private void addShellCase(bool isFacingRight)
    {
        if (ShellCase != null)
        {
            Vector3 shellPos = ShootPoint.position;
            Rigidbody2D shellCase = Instantiate(ShellCase, shellPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0.0f, 180.0f)))) as Rigidbody2D;
            shellCase.velocity = new Vector2(isFacingRight ? Random.Range(-4.0f, -2.0f) : Random.Range(2.0f, 4.0f), Random.Range(2.0f, 4.0f));
            shellCase.angularVelocity = 360.0f;
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

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0) 
            return;
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
        
            boxCollider.enabled = false;
            deadDirection = direction;
            stateMachine.ChangeState(GatelingJackDeadState.Instance);
        }
        else
        {

            // 受伤变红
            StartCoroutine("HurtColorChange");
          
            addHurtEffect(pos, direction > 0 ? true : false);
        }

    }

    public override void Dead()
    {
        //StartSlowMotion();
        StopCoroutine("spawn");
        addDeadEffect(boxCollider.bounds.center);
        Animator.SetBool("Attacking", false);
        Animator.SetBool("Run", false);
        Animator.SetBool("Dead", true);
        HitBack(deadDirection >= 0 ? 2.5f : -2.5f, 5.0f, 0.1f);

        Disapper();
    }

    public bool Help()
    {
        CallForHelp();
        pauseTimer += Time.deltaTime;
        Animator.SetBool("Help", true);
        if (pauseTimer >= PauseTime && HelperIndex == HelperCount)
        {
            pauseTimer = 0;
            Animator.SetBool("Help", false);
            currentPoint.MoveNext();
            return true;
        }

        return false;
    }

    public void ResetHelp()
    {
        HelperIndex = 0;
        HelperState = false;
    }

    public void CallForHelp()
    {
        if (!HelperState)
        {
            HelperState = true;
            if (CurrentHealth >= 0.5f * MaxHealth)
            {
                HelperCount = 1;
                PauseTime = 2.0f;
            }
            else if (CurrentHealth >= 0.25f * MaxHealth && CurrentHealth < 0.5f * MaxHealth)
            {
                HelperCount = 2;
                PauseTime = 3.0f;
            }
            else if (CurrentHealth > 0.0f && CurrentHealth < 0.25f * MaxHealth)
            {
                HelperCount = 3;
                PauseTime = 4.0f;
            }
            
            StartCoroutine("spawn");
        }
        
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(1.0f);

        if (HelperIndex < HelperCount)
        {

            GameObject helper = Instantiate(Helper, (isFacingRight ? SpawnPoints[0].transform.position : SpawnPoints[1].transform.position), transform.rotation) as GameObject;

            if (helper != null)
            {
                HelperIndex++;
                Destroyer destroyer = helper.transform.GetComponent<Destroyer>();
                destroyer.Target = Player;
                destroyer.Direction = isFacingRight ? -1 : 1;
                destroyer.GetFSM().ChangeState(DestroyerCrazyState.Instance);
                StartCoroutine(spawn());
            }

        }
        
    }
}
