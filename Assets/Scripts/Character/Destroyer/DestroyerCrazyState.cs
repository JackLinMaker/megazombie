using UnityEngine;
using System.Collections;

public class DestroyerCrazyState : State<Destroyer>
{
    private static readonly DestroyerCrazyState instance = new DestroyerCrazyState();

    public static DestroyerCrazyState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Destroyer entity)
    {
        entity.Animator.SetBool("Run", true);
        entity.Perspective.enabled = false;
        entity.Flip(entity.Direction);
        entity.ShowExclamation();

    }

    public override void Execute(Destroyer entity)
    {
        entity.Controller.SetHorizontalForce(entity.Direction == 1 ? entity.SpeedAccelerationOnGround : -entity.SpeedAccelerationOnGround);
    }

    public override void Exit(Destroyer entity)
    {
       
    }
	
}
