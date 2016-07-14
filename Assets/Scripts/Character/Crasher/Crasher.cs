using UnityEngine;
using System.Collections;

public class Crasher : BaseEntity
{
    public Collider2D Blood;
    public Collider2D[] BodyPiece;

    private StateMachine<Crasher> stateMachine;

    #region SystemFunc

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Crasher>(this);
        stateMachine.ChangeState(CrasherIdleState.Instance);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    #endregion

    public StateMachine<Crasher> GetFSM()
    {
        return stateMachine;
    }

    public override void Attack()
    {

    }

    public override void Hurt(float damage, Vector3 pos, int direction)
    {
        if (CurrentHealth <= 0) return;
        SoundManager.Instance.MakeHitZombieSound(this.transform.position);
        addHurtEffect(pos, direction > 0 ? true : false);
        AddBlood(pos, direction > 0 ? true : false);
        StartCoroutine("HurtColorChange");
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            deadDirection = direction;
            Dead(pos);
            BaseUI.Instance.AddKilledEnemy(this.gameObject.tag);
        }
        else
        {
            if (stateMachine.IsInState(CrasherIdleState.Instance))
            {

                perspective.Detected = true;
                stateMachine.ChangeState(CrasherAttackState.Instance);
            }
        }

    }


    private void AddBlood(Vector3 pos, bool isFaceRight)
    {
        int count = Random.Range(2, 4);
        for (int i = 0; i < count; i++)
        {
            Collider2D col = Instantiate(Blood, pos + new Vector3(Random.Range(-0.1f, 0.1f) + (isFaceRight ? 1 : -1) * 0.2f, Random.Range(-0.1f, 0.1f)), Quaternion.identity) as Collider2D;
            Vector3 force = (col.transform.position - pos) * Random.Range(15, 25);
            col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(force.x, force.y);
        }
    }


    public void Dead(Vector3 pos)
    {
        Director.Instance.ShakeScreen();
        perspective.enabled = false;
        addDeadEffect(pos);
        int count = Random.Range(2, 3);
        for (int i = 0; i < count; i++)
        {
            Collider2D col = Instantiate(Blood, transform.position + new Vector3(Random.Range(-0.1f, 0.1f) + deadDirection * 0.1f, Random.Range(0.1f, 0.4f) / Mathf.Abs(deadDirection * 2)), Quaternion.identity) as Collider2D;

            Vector3 force = (col.transform.position - transform.position) * Random.Range(15, 35);
            col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(force.x, force.y);
        }

        for (int i = 0; i < BodyPiece.Length; i++)
        {
            Collider2D piece = Instantiate(BodyPiece[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as Collider2D;
            piece.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(1.0f, 5.0f) * deadDirection, Random.Range(3.0f, 6.0f)) + Controller.Velocity;
        }

        Destroy(this.gameObject);
    }

    public override void ResetAttackEvent()
    {
        isAttacking = false;
        controller.Animator.SetBool("Attacking", false);
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
