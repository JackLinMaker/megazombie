using UnityEngine;
using System.Collections;

public class ShielderChaseState : State<Shielder> {

    private static readonly ShielderChaseState instance = new ShielderChaseState();

    public static ShielderChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime;

    public override void Enter(Shielder entity)
    {
        entity.ChaseFactor = 2.0f;
        entity.Animator.SetBool("Run", true);
        entity.ShowExclamation();
    }

    public override void Execute(Shielder entity)
    {
        if (entity.Target == null)
        {
            entity.GetComponent<Perspective>().Detected = false;
            entity.GetFSM().ChangeState(ShielderIdleState.Instance);

            

          
        }
        else
        {

            if ((entity.Target.transform.GetComponent<Player>() != null && entity.Target.transform.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.transform.GetComponent<BaseEntity>() != null && entity.Target.transform.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(ShielderIdleState.Instance);

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
                        entity.GetFSM().ChangeState(ShielderPatrolState.Instance);
                    }
                    else
                    {
                        entity.GetFSM().ChangeState(ShielderIdleState.Instance);
                    }


                }
                else if (distanceX > entity.AttackRange && distanceX <= entity.ChaseRange)
                {
                    entity.Chase();
                }
                else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
                {

                    entity.GetFSM().ChangeState(ShielderDefenseState.Instance);
                }
            }

                
            
        }

       
        
       
       
    }

    public override void Exit(Shielder entity)
    {
        entity.Animator.SetBool("Run", false);
        entity.ChaseFactor = 1.0f;
    }
}
