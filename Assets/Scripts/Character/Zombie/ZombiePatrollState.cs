using UnityEngine;
using System.Collections;

public sealed class ZombiePatrollState : State<Zombie>
{
    private static readonly ZombiePatrollState instance = new ZombiePatrollState();

    public static ZombiePatrollState Instance
    {
        get
        {
            return instance;
        }
    }
    
    public override void Enter(Zombie entity)
    {
        entity.Animator.SetBool("Walk", true);
    }

    public override void Execute(Zombie entity)
    {
        if (entity.Perspective.Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(ZombieChaseState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {
                entity.GetFSM().ChangeState(ZombiePauseState.Instance);
            }
            else
            {
                entity.Patrol();
            }
        }
       
      
    }

    public override void Exit(Zombie entity)
    {
        entity.Animator.SetBool("Walk", false);
       
    }

}
