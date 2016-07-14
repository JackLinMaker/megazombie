using UnityEngine;
using System.Collections;

public class SergentDanDeadState : State<SergentDan>
{

    private static readonly SergentDanDeadState instance = new SergentDanDeadState();

    public static SergentDanDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(SergentDan entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(SergentDan entity)
    {
       
    }

    public override void Exit(SergentDan entity)
    {
       
    }
}
