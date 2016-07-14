using UnityEngine;
using System.Collections;

public class HeavyMachinerDeadState : State<HeavyMachiner>
{
    private static readonly HeavyMachinerDeadState instance = new HeavyMachinerDeadState();

    public static HeavyMachinerDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(HeavyMachiner entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(HeavyMachiner entity)
    {

    }

    public override void Exit(HeavyMachiner entity)
    {

    }
}
