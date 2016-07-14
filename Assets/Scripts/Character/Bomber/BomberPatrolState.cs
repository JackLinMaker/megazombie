using UnityEngine;
using System.Collections;

public class BomberPatrolState : State<Bomber>
{

    private static readonly BomberPatrolState instance = new BomberPatrolState();

    public static BomberPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bomber entity)
    {
       
        entity.Animator.SetBool("Run", true);
       
    }

    public override void Execute(Bomber entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(BomberChaseState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {

                entity.GetFSM().ChangeState(BomberPauseState.Instance);
            }
            else
            {
                entity.Patrol();
            }

        }
    }

    public override void Exit(Bomber entity)
    {
        entity.Animator.SetBool("Run", false);
    }
}
