using UnityEngine;
using System.Collections;

public class SergentDanIdleState : State<SergentDan> 
{

	private static readonly SergentDanIdleState instance = new SergentDanIdleState();

    public static SergentDanIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(SergentDan entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(SergentDan entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(SergentDanChargeState.Instance);
        }
    }

    public override void Exit(SergentDan entity)
    { 
    
    }
        
}
