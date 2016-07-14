using UnityEngine;
using System.Collections;

public class SergentDanLRState : State<SergentDan>
{
    private static readonly SergentDanLRState instance = new SergentDanLRState();

    public static SergentDanLRState Instance
    {
        get
        {
            return instance;
        }
    }

    private int attackCount = 0;
    private float elapsedTime;

    public override void Enter(SergentDan entity)
    {
        Debug.Log("Enter LR");
        elapsedTime = 0.5f;
        attackCount = 3;
    }

    public override void Execute(SergentDan entity)
    {
        
        if (attackCount <= 0 && entity.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.idle"))
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 3f)
            {
                entity.GetFSM().ChangeState(SergentDanChargeState.Instance);
            }
        }
        else
        {
            if (attackCount > 0)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 0.8f)
                {
                    elapsedTime = 0.0f;
                    entity.FaceToTarget();
                    entity.Animator.Play("Base Layer.LRAttack", 0, 0);
                    attackCount -= 1;
                }
            }
        }


    }

    public override void Exit(SergentDan entity)
    {
        Debug.Log("Exit LR");
    }
}
