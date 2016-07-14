using UnityEngine;
using System.Collections;

public class DestroyerPatrolState : State<Destroyer>
{
    private static readonly DestroyerPatrolState instance = new DestroyerPatrolState();

    public static DestroyerPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Destroyer entity)
    {

    }

    public override void Execute(Destroyer entity)
    {
        if (entity.GetComponent<Perspective>().Detected && entity.Target.position.x > entity.PatrolPath.GetMinXPoint().position.x && entity.Target.position.x < entity.PatrolPath.GetMaxXPoint().position.x)
        {
            entity.GetFSM().ChangeState(DestroyerChaseState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {

                entity.currentPoint.MoveNext();
            }
            else
            {
                entity.Patrol();
            }
            entity.GetComponent<Perspective>().Detected = false;
        }
    }

    public override void Exit(Destroyer entity)
    {
        entity.Animator.SetBool("Run", false);
    }

}
