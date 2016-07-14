using UnityEngine;
using System.Collections;

public class BomberDeadState : State<Bomber>
{
    private static readonly BomberDeadState instance = new BomberDeadState();

    public static BomberDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bomber entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(Bomber entity)
    {
       
    }

    public override void Exit(Bomber entity)
    {
       
    }
}
