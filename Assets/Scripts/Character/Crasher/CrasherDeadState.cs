using UnityEngine;
using System.Collections;

public class CrasherDeadState : State<Crasher>
{
    private static readonly CrasherDeadState instance = new CrasherDeadState();

    public static CrasherDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Crasher entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(Crasher entity)
    {

    }

    public override void Exit(Crasher entity)
    {

    }
}
