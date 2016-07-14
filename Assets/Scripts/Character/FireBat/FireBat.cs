using UnityEngine;
using System.Collections;

public class FireBat : BaseEntity
{
    public Collider2D[] BodyPiece;
    public float AttackRate = 2.5f;
    public GameObject FireBall;
    public Transform MouthPositon;
    public float FireBallRange = 5.0f;
    public Vector3 StartPoint;
    private StateMachine<FireBat> stateMachine;
    private Vector2 randomPosition = Vector2.zero;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<FireBat>(this);
        stateMachine.ChangeState(FireBatPatrolState.Instance);
        StartPoint = this.transform.position;
        AttackDuration = AttackRate;
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<FireBat> GetFSM()
    {
        return stateMachine;
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
        SoundManager.Instance.MakeHitZombieSound(this.transform.position);
        addHurtEffect(pos, direction >= 0 ? true : false);
        StartCoroutine("HurtColorChange");
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            deadDirection = direction;
            stateMachine.ChangeState(FireBatDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            if (stateMachine.IsInState(FireBatPatrolState.Instance) || stateMachine.IsInState(FireBatPauseState.Instance))
            {
                Player player = Target.GetComponent<Player>();
                if (player != null && player.CurrentHealth > 0)
                {
                    Debug.Log("BatChaseState");
                    perspective.Detected = true;
                    stateMachine.ChangeState(FireBatChaseState.Instance);
                }
            }
        }


    }

    public override void Dead()
    {
        Director.Instance.ShakeScreen();
        perspective.enabled = false;
        addDeadEffect(transform.position);

        for (int i = 0; i < BodyPiece.Length; i++)
        {
            Collider2D piece = Instantiate(BodyPiece[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as Collider2D;
            piece.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(1.0f, 5.0f) * deadDirection, Random.Range(3.0f, 6.0f)) + Controller.Velocity;
        }

        Destroy(gameObject);
    }

    public void Fly()
    {
        if (randomPosition == Vector2.zero)
        {
            randomPosition = new Vector2(Target.position.x + Random.Range(-2f, 2f), Target.position.y + Random.Range(1.5f, 2.8f));
        }
        else
        {
            Vector2 diff = randomPosition - (Vector2)transform.position;
            Vector2 face = Target.transform.position - transform.position;
            Flip(face.x);
            this.GetComponent<Rigidbody2D>().velocity = diff.normalized * SpeedAccelerationInAir;
        }

        float dis = Vector3.Distance(transform.position, randomPosition);
        if (dis < 0.1f)
        {
            randomPosition = Vector2.zero;
        }
    }


    public void Back()
    {
        Vector2 direction = (StartPoint - transform.position).normalized;

        if (direction.x >= 0)
        {
            Flip(1);
        }
        else if (direction.x < 0)
        {
            Flip(-1);
        }

        transform.GetComponent<Rigidbody2D>().velocity = direction * SpeedAccelerationInAir;
    }

    public void Attack()
    {

        GameObject fireball = Instantiate(FireBall, MouthPositon.position + new Vector3(0, 0.3f, 0), Quaternion.identity) as GameObject;
        BaseBullet bullet = fireball.transform.GetComponent<BaseBullet>();
        float angle = Mathf.Rad2Deg * Mathf.Atan((Target.position.y - transform.position.y) / (Target.position.x - transform.position.x));
        if (Target.position.x - transform.position.x < 0)
            angle = angle - 180;

        bullet.transform.localEulerAngles = new Vector3(0, 0, angle);

        bullet.Speed = 4.0f;
        bullet.Damage = Damage;
        bullet.IsFacingRight = isFacingRight;
        bullet.Range = FireBallRange;
        bullet.Owner = Util.ObjectOwner.Zombie;
        bullet.Host = this.transform;
        bullet.Direction = Vector2.right;
        AttackDuration = AttackRate;
        Animator.SetBool("Attacking", false);
    }
}
