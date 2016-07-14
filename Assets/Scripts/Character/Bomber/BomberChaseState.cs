using UnityEngine;
using System.Collections;

public class BomberChaseState : State<Bomber>
{

    private static readonly BomberChaseState instance = new BomberChaseState();

    public static BomberChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bomber entity)
    {
        entity.Animator.SetBool("Run", true);
        entity.ShowExclamation();
    }

    public override void Execute(Bomber entity)
    {
        if (entity.Target == null)
        {
            
            entity.GetComponent<Perspective>().Detected = false;
            entity.GetFSM().ChangeState(BomberIdleState.Instance);
        }
        else
        {
            if ((entity.Target.transform.GetComponent<Player>() != null && entity.Target.transform.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.transform.GetComponent<BaseEntity>() != null && entity.Target.transform.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(BomberIdleState.Instance);
                entity.GetComponent<Perspective>().Detected = false;
                
            }
            else
            {
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);
                if (distanceX > entity.ChaseRange || distanceY > entity.AttackRange)
                {
                    entity.Target = null;
                    entity.GetComponent<Perspective>().Detected = false;
                    entity.ShowQuestion();
                    if (entity.PatrolPath != null)
                    {
                        entity.GetFSM().ChangeState(BomberPatrolState.Instance);
                    }
                    else
                    {
                        entity.GetFSM().ChangeState(BomberIdleState.Instance);
                    }


                }
                else if (distanceX > entity.AttackRange && distanceX <= entity.ChaseRange)
                {
                    entity.Chase();
                }
                else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
                {
                    
                    entity.Controller.SetHorizontalForce(0.0f);
                    entity.GetFSM().ChangeState(BomberAttackState.Instance);
                }
            }

            
        }
       
       
    }

    public override void Exit(Bomber entity)
    {
        entity.Animator.SetBool("Run", false);
    }
}
