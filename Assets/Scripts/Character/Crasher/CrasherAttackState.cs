using UnityEngine;
using System.Collections;

public class CrasherAttackState : State<Crasher>
{
 
    private static readonly CrasherAttackState instance = new CrasherAttackState();

    public static CrasherAttackState Instance
    {
        get
        {
            return instance;
        }
    }

    private float sign;


    public override void Enter(Crasher entity)
    {
        entity.Animator.SetBool("Run", true);
        entity.FaceToTarget();
        entity.ShowExclamation();
        sign = Mathf.Sign(entity.Target.transform.position.x - entity.transform.position.x);
    }

    public override void Execute(Crasher entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
            float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);
            if (distanceX > entity.ChaseRange || distanceY > entity.ChaseRange)
            {
                entity.Target = null;
                entity.Perspective.Detected = false;
                entity.GetFSM().ChangeState(CrasherIdleState.Instance);
            }
            else
            {
                if (entity.Animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    Transform point;
                    if (sign >= 0)
                    {
                        point = entity.PatrolPath.GetMaxXPoint();
                    }
                    else
                    {
                        point = entity.PatrolPath.GetMinXPoint();
                    }

                    /*if (entity.ReachDestination(point))
                    {
                        entity.Controller.SetHorizontalForce(0.0f);
                        entity.Animator.SetBool("Run", false);
                        entity.Perspective.Detected = false;
                        entity.Target = null;
                        entity.GetFSM().ChangeState(CrasherIdleState.Instance);
                    }
                    else
                    {
                        entity.moveToDestination(point);
                    }*/

                    if (entity.ReachDestination(entity.currentPoint.Current))
                    {
                        entity.currentPoint.MoveNext();
                    }
                    else
                    {
                        entity.Patrol();
                    }
                }
            }

        }
        else
        {
            entity.Target = null;
            entity.Perspective.Detected = false;
            entity.GetFSM().ChangeState(CrasherIdleState.Instance);
        }
        
    }

    public override void Exit(Crasher entity)
    {
        entity.Animator.SetBool("Run", false);
    }
}
