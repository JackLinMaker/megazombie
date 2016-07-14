using UnityEngine;
using System.Collections;

public class SoldierPatrollState : State<Soldier> 
{
    private static readonly SoldierPatrollState instance = new SoldierPatrollState();

    public static SoldierPatrollState Instance
    {
        get
        {
            return instance;
        }
    }


    public override void Enter(Soldier entity)
    {
        entity.Animator.SetFloat("Speed", 1);
      
    }

    public override void Execute(Soldier entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(SoldierChaseState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {
                entity.GetFSM().ChangeState(SoldierPauseState.Instance);
            }
            else
            {
                entity.Patrol();
            }
           
        }
    }

    public override void Exit(Soldier entity)
    {
        
    }
	
}
