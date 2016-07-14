using UnityEngine;
using System.Collections;

public class SoldierHurtState : State<Soldier>
{

    private static readonly SoldierHurtState instance = new SoldierHurtState();

    public static SoldierHurtState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Soldier entity)
    {
        
    }

    public override void Execute(Soldier entity)
    { 
    
    }

    public override void Exit(Soldier entity)
    {

    }
}
