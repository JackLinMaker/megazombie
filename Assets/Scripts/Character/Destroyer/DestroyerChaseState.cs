using UnityEngine;
using System.Collections;

public class DestroyerChaseState : State<Destroyer>
{
    private static readonly DestroyerChaseState instance = new DestroyerChaseState();

    public static DestroyerChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Destroyer entity)
    {
        entity.ShowExclamation();
    }

    public override void Execute(Destroyer entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
            float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);
            if (player.transform.position.x <= entity.PatrolPath.GetMinXPoint().position.x || player.transform.position.x >= entity.PatrolPath.GetMaxXPoint().position.x)
            {
                entity.Target = null;
                entity.GetComponent<Perspective>().Detected = false;
                entity.ShowQuestion();
                entity.GetFSM().ChangeState(DestroyerPatrolState.Instance);
            }
            else if (distanceX > entity.AttackRange && distanceX <= entity.ChaseRange)
            {
                entity.Chase();
            }
            else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
            {

                entity.Controller.SetHorizontalForce(0.0f);
                entity.GetFSM().ChangeState(DestroyerDeadState.Instance);
            }
        }
        else
        {
            entity.Target = null;
            entity.Perspective.Detected = false;
            entity.GetFSM().ChangeState(DestroyerPatrolState.Instance);
        }
    }




    public override void Exit(Destroyer entity)
    {
        entity.Animator.SetBool("Run", false);
    }

}
