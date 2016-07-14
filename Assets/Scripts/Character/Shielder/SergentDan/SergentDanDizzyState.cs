using UnityEngine;
using System.Collections;

public class SergentDanDizzyState : State<SergentDan>
{

    private static readonly SergentDanDizzyState instance = new SergentDanDizzyState();

    public static SergentDanDizzyState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime = 0;

    public override void Enter(SergentDan entity)
    {
        Debug.Log("Enter Dead");
        elapsedTime = 2f;
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Animator.SetBool("Dizzy", true);
    }

    public override void Execute(SergentDan entity)
    {
        elapsedTime -= Time.deltaTime;
        if (elapsedTime <= 0)
        {
            entity.GetFSM().ChangeState(SergentDanChargeState.Instance);
        }
    }

    public override void Exit(SergentDan entity)
    {
        entity.Animator.SetBool("Dizzy", false);
    }
}
