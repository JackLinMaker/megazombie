using UnityEngine;
using System.Collections;

public class ShielderIdleState : State<Shielder>
{

    private static readonly ShielderIdleState instance = new ShielderIdleState();

    public static ShielderIdleState Instance
    {
        get
        {
            return instance;
        }
    }


    private float elapsedTime = 0.0f;

    public override void Enter(Shielder entity)
    {
        entity.Animator.SetBool("Run", false);
        entity.Controller.SetHorizontalForce(0.0f);

    }

    public override void Execute(Shielder entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(ShielderDefenseState.Instance);
        }
        else
        {

        }
    }

    public override void Exit(Shielder entity)
    {

    }
}
