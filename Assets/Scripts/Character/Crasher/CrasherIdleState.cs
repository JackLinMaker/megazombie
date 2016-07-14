using UnityEngine;
using System.Collections;

public class CrasherIdleState : State<Crasher>
{
    private static readonly CrasherIdleState instance = new CrasherIdleState();

    public static CrasherIdleState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime = 0.0f;

    public override void Enter(Crasher entity)
    {
        entity.Animator.SetBool("Run", false);
        entity.Controller.SetHorizontalForce(0.0f);
    }

    public override void Execute(Crasher entity)
    {
        if (entity.Perspective.Detected == true && entity.Target.position.x > entity.PatrolPath.GetMinXPoint().position.x && entity.Target.position.x < entity.PatrolPath.GetMaxXPoint().position.x)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(CrasherAttackState.Instance);
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

    public override void Exit(Crasher entity)
    {

    }
}
