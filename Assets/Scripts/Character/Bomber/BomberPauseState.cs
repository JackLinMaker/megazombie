using UnityEngine;
using System.Collections;

public class BomberPauseState : State<Bomber>
{

    private static readonly BomberPauseState instance = new BomberPauseState();

    public static BomberPauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bomber entity)
    {
        
        entity.Animator.SetBool("Run", false);

    }

    public override void Execute(Bomber entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(BomberChaseState.Instance);
        }
        else
        {
            if (entity.Pause())
            {
                entity.GetFSM().ChangeState(BomberPatrolState.Instance);
            }
        }
    }

    public override void Exit(Bomber entity)
    {

    }
}
