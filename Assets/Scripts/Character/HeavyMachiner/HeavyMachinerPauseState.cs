using UnityEngine;
using System.Collections;

public class HeavyMachinerPauseState : State<HeavyMachiner>
{
    private static readonly HeavyMachinerPauseState instance = new HeavyMachinerPauseState();

    public static HeavyMachinerPauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(HeavyMachiner entity)
    {
        entity.Animator.SetBool("Walk", false);
    }

    public override void Execute(HeavyMachiner entity)
    {

        if (entity.Pause())
        {
            entity.GetFSM().ChangeState(HeavyMachinerPatrolState.Instance);
        }
    }

    public override void Exit(HeavyMachiner entity)
    {
        
    }
}
