using UnityEngine;
using System.Collections;

public class ShielderDeadState : State<Shielder> {

    private static readonly ShielderDeadState instance = new ShielderDeadState();

    public static ShielderDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Shielder entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(Shielder entity)
    {

    }

    public override void Exit(Shielder entity)
    {

    }
}
