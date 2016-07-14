using UnityEngine;
using System.Collections;

public class MissileCarIdleState : State<MissileCar>
{

    private static readonly MissileCarIdleState instance = new MissileCarIdleState();

    public static MissileCarIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(MissileCar entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(MissileCar entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(MissileCarAttackState.Instance);
        }
    }

    public override void Exit(MissileCar entity)
    {
       
    }
}
