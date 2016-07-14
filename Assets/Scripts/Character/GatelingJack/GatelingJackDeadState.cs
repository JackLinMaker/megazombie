using UnityEngine;
using System.Collections;

public class GatelingJackDeadState : State<GatelingJack>
{

    private static readonly GatelingJackDeadState instance = new GatelingJackDeadState();

    public static GatelingJackDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(GatelingJack entity)
    {

        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(GatelingJack entity)
    {

    }

    public override void Exit(GatelingJack entity)
    {

    }
}
