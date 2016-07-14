using UnityEngine;
using System.Collections;

public class MissileCarAttackState : State<MissileCar>
{
    private static readonly MissileCarAttackState instance = new MissileCarAttackState();

    public static MissileCarAttackState Instance
    {
        get
        {
            return instance;
        }
    }
    private float attackDuration = 0.0f;
    public override void Enter(MissileCar entity)
    {
        entity.Animator.SetBool("Attacking", true);
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(MissileCar entity)
    {
        if (entity.Target == null)
        {
            entity.GetFSM().ChangeState(MissileCarIdleState.Instance);

            entity.GetComponent<Perspective>().Detected = false;
        }
        else
        {
            if ((entity.Target.transform.GetComponent<Player>() != null && entity.Target.transform.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.transform.GetComponent<BaseEntity>() != null && entity.Target.transform.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(MissileCarIdleState.Instance);

                entity.GetComponent<Perspective>().Detected = false;

            }
            else
            {
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

                if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
                {
                    attackDuration -= Time.deltaTime;
                    if (attackDuration <= 0)
                    {
                        attackDuration = 2f;
                        entity.TriggerAttackEvent();
                    }

                }
                else if (distanceX > entity.AttackRange || distanceY > entity.AttackRange)
                {
                    entity.GetFSM().ChangeState(MissileCarIdleState.Instance);
                    entity.GetComponent<Perspective>().Detected = false;
                }
            }
        }
    }

    public override void Exit(MissileCar entity)
    {

    }
}
