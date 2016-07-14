using UnityEngine;
using System.Collections;

public class RobotGunAttackState : State<RobotGun>
{

    private static readonly RobotGunAttackState instance = new RobotGunAttackState();

    public static RobotGunAttackState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(RobotGun entity)
    {
        entity.Animator.SetBool("Attacking", true);
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(RobotGun entity)
    {
        if (entity.Target == null)
        {
            entity.GetFSM().ChangeState(RobotGunIdleState.Instance);

            entity.GetComponent<Perspective>().Detected = false;
        }
        else
        {
            if ((entity.Target.transform.GetComponent<Player>() != null && entity.Target.transform.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.transform.GetComponent<BaseEntity>() != null && entity.Target.transform.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(RobotGunIdleState.Instance);

                entity.GetComponent<Perspective>().Detected = false;

            }
            else
            {
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

                if (distanceX > entity.AttackRange || distanceY > 1.5)
                {
                    entity.GetFSM().ChangeState(RobotGunIdleState.Instance);
                    entity.GetComponent<Perspective>().Detected = false;
                }
            }
        }
    }

    public override void Exit(RobotGun entity)
    {

    }
}
