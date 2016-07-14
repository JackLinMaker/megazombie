using UnityEngine;
using System.Collections;

public class ShielderPatrolState : State<Shielder> {

    private static readonly ShielderPatrolState instance = new ShielderPatrolState();

    public static ShielderPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Shielder entity)
    {
        entity.Animator.SetBool("Walk", true);
    }

    public override void Execute(Shielder entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(ShielderChaseState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {

                entity.GetFSM().ChangeState(ShielderPauseState.Instance);
            }
            else
            {
                entity.Patrol();
            }
        }
        
    }

    public override void Exit(Shielder entity)
    {

    }
}
