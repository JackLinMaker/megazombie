using UnityEngine;
using System.Collections;

public class HeavyMachinerPatrolState : State<HeavyMachiner>
{
    private static readonly HeavyMachinerPatrolState instance = new HeavyMachinerPatrolState();

    public static HeavyMachinerPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(HeavyMachiner entity)
    {
        entity.Animator.SetBool("Walk", true);
    }

    public override void Execute(HeavyMachiner entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(HeavyMachinerAttackState.Instance);
        }
        else
        {
            if (entity.ReachDestination(entity.currentPoint.Current))
            {

                entity.GetFSM().ChangeState(HeavyMachinerPauseState.Instance);
            }
            else
            {
                entity.Patrol();
            }
        }
    }

    public override void Exit(HeavyMachiner entity)
    {

    }
}
