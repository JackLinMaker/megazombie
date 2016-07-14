using UnityEngine;
using System.Collections;

public class GatelingJackSleepState : State<GatelingJack>
{

    private static readonly GatelingJackSleepState instance = new GatelingJackSleepState();

    public static GatelingJackSleepState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(GatelingJack entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(GatelingJack entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(GatelingJackChaseState.Instance);
        }
    }

    public override void Exit(GatelingJack entity)
    {
        
    }
}
