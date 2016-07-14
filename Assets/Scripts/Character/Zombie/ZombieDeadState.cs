using UnityEngine;
using System.Collections;

public class ZombieDeadState : State<Zombie>  
{
    private static readonly ZombieDeadState instance = new ZombieDeadState();

    public static ZombieDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Zombie entity)
    {
        
        entity.Controller.SetHorizontalForce(0.0f);
        //entity.Dead();
    }

    public override void Execute(Zombie entity)
    {
       
        
    }

    public override void Exit(Zombie entity)
    {
        Debug.Log("Exit Head Dead");
    }

}
