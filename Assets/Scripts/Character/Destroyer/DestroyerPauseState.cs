using UnityEngine;
using System.Collections;

public class DestroyerPauseState : State<Destroyer>
{

    private static readonly DestroyerPauseState instance = new DestroyerPauseState();

    public static DestroyerPauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Destroyer entity)
    {

        entity.Animator.SetBool("Run", false);
        entity.Controller.SetHorizontalForce(0.0f);

    }

    public override void Execute(Destroyer entity)
    {
        if (entity.GetComponent<Perspective>().Detected && entity.Target.position.x > entity.PatrolPath.GetMinXPoint().position.x && entity.Target.position.x < entity.PatrolPath.GetMaxXPoint().position.x)
        {
            entity.GetFSM().ChangeState(DestroyerChaseState.Instance);
        }
        else
        {
            if (entity.Pause())
            {
                entity.GetFSM().ChangeState(DestroyerPatrolState.Instance);
            }
            entity.GetComponent<Perspective>().Detected = false;
        }
    }

    public override void Exit(Destroyer entity)
    {

    }
}
