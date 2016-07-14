using UnityEngine;
using System.Collections;

public class RobotGunIdleState : State<RobotGun>
{
    private static readonly RobotGunIdleState instance = new RobotGunIdleState();

    public static RobotGunIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(RobotGun entity)
    {
        entity.Animator.SetBool("Attacking", false);
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(RobotGun entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(RobotGunAttackState.Instance);
        }
    }

    public override void Exit(RobotGun entity)
    {
       
    }
}
