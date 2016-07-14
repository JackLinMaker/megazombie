using UnityEngine;
using System.Collections;

public class ShielderPauseState : State<Shielder> {

    private static readonly ShielderPauseState instance = new ShielderPauseState();

    public static ShielderPauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Shielder entity)
    {
        entity.Animator.SetBool("Walk", false);
    }

    public override void Execute(Shielder entity)
    {
        
        if (entity.Pause())
        {
            entity.GetFSM().ChangeState(ShielderPatrolState.Instance);
        }
    }

    public override void Exit(Shielder entity)
    {

    }
}
