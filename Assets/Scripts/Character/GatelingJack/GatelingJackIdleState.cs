using UnityEngine;
using System.Collections;

public class GatelingJackIdleState : State<GatelingJack>
{
    private static readonly GatelingJackIdleState instance = new GatelingJackIdleState();

    public static GatelingJackIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(GatelingJack entity)
    {
        entity.Animator.SetBool("Run", false);
        entity.Animator.SetBool("Attacking", false);
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Flip(entity.isFacingRight ? -1 : 1);
        entity.ResetHelp();
       
    }

    public override void Execute(GatelingJack entity)
    {
      
        if (entity.Help())
        {
            entity.GetFSM().ChangeState(GatelingJackChaseState.Instance);
        }
        
    }

    public override void Exit(GatelingJack entity)
    {
        
    }
}
