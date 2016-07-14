using UnityEngine;
using System.Collections;

public class HeavyMachinerIdleState : State<HeavyMachiner>
{

    private static readonly HeavyMachinerIdleState instance = new HeavyMachinerIdleState();

    public static HeavyMachinerIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime = 0.0f;

    public override void Enter(HeavyMachiner entity)
    {
        entity.Animator.SetBool("Attacking", false);
        entity.Animator.SetBool("Walk", false);
        entity.Controller.SetHorizontalForce(0.0f);

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

        }
    }

    public override void Exit(HeavyMachiner entity)
    {

    }
}
