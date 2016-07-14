using UnityEngine;
using System.Collections;

public class DestroyerIdleState : State<Destroyer>
{
    private static readonly DestroyerIdleState instance = new DestroyerIdleState();

    public static DestroyerIdleState Instance
    {
        get
        {
            return instance;
        }
    }


    private float elapsedTime = 0.0f;

    public override void Enter(Destroyer entity)
    {
        entity.Animator.SetBool("Run", false);
        entity.Controller.SetHorizontalForce(0.0f);

    }

    public override void Execute(Destroyer entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(DestroyerChaseState.Instance);
        }
        else
        {


            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 2.0f)
            {
                elapsedTime = 0.0f;
                entity.Flip(entity.isFacingRight ? -1 : 1);
            }

        }
    }

    public override void Exit(Destroyer entity)
    {

    }

}
