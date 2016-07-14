using UnityEngine;
using System.Collections;

public class SoldierDeadState : State<Soldier> 
{
    private static readonly SoldierDeadState instance = new SoldierDeadState();

    public static SoldierDeadState Instance
    {
        get
        {
            return instance;
        }
    }


    public override void Enter(Soldier entity)
    {
       
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(Soldier entity)
    {
        
    }

    public override void Exit(Soldier entity)
    {
        
    }
}
