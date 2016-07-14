using UnityEngine;
using System.Collections;

public class Bat : BaseEntity 
{
    public Collider2D[] BodyPiece;

    private StateMachine<Bat> stateMachine;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Bat>(this);
        stateMachine.ChangeState(BatPatrollState.Instance);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public StateMachine<Bat> GetFSM()
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
            stateMachine.ChangeState(BatDeadState.Instance);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            if (stateMachine.IsInState(BatPatrollState.Instance) || stateMachine.IsInState(BatPauseState.Instance))
            {
                Player player = Target.GetComponent<Player>();
                if (player != null && player.CurrentHealth > 0)
                {
                    Debug.Log("BatChaseState");
                    perspective.Detected = true;
                    stateMachine.ChangeState(BatChaseState.Instance);
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
        if (currentPoint == null || currentPoint.Current == null)
            return;

        Vector2 direction = (currentPoint.Current.position - transform.position).normalized;
      
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
}
