using UnityEngine;
using System.Collections;

public class DestroyerDeadState : State<Destroyer>
{
    private static readonly DestroyerDeadState instance = new DestroyerDeadState();

    public static DestroyerDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Destroyer entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Animator.SetBool("Dead", true);
    }

    public override void Execute(Destroyer entity)
    {
        
    }

    public override void Exit(Destroyer entity)
    {

    }
	
}
