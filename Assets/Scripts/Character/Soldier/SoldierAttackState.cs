using UnityEngine;
using System.Collections;

public class SoldierAttackState : State<Soldier>
{
    private static readonly SoldierAttackState instance = new SoldierAttackState();

    public static SoldierAttackState Instance
    {
        get
        {
            return instance;
        }
    }

  
    public override void Enter(Soldier entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Animator.SetFloat("Speed", 0.0f);
    }

    public override void Execute(Soldier entity)
    {
         Player player = entity.Target.transform.GetComponent<Player>();
         if (player != null && player.CurrentHealth > 0)
         {
            
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);
                AnimatorStateInfo asi = entity.Animator.GetCurrentAnimatorStateInfo(0);
                if ((distanceX > entity.AttackRange || distanceY > entity.AttackRange) && !asi.IsName("Base Layer.attack") && !asi.IsName("Base Layer.idle_attack"))
                {
                    entity.GetFSM().ChangeState(SoldierChaseState.Instance);
                }
                else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
                {
                    entity.AttackDuration -= Time.deltaTime;
                    if (entity.AttackDuration <= 0)
                    {
                        entity.AttackDuration = 1.5f;
                        entity.TriggerAttackEvent();
                    }

                }
             
         }
         else
         {
             entity.Target = null;
             entity.Perspective.Detected = false;
             if (entity.PatrolPath != null)
             {
                 entity.GetFSM().ChangeState(SoldierPatrollState.Instance);
             }
         }


        /*if (entity.Target == null)
        {
            entity.GetFSM().ChangeState(SoldierIdleState.Instance);

            entity.GetComponent<Perspective>().Detected = false;
        }
        else
        {
            if ((entity.Target.transform.GetComponent<Player>() != null && entity.Target.transform.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.transform.GetComponent<BaseEntity>() != null && entity.Target.transform.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(SoldierIdleState.Instance);

                entity.GetComponent<Perspective>().Detected = false;

            }
            else
            {
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);
                AnimatorStateInfo asi = entity.Animator.GetCurrentAnimatorStateInfo(0);
                if ((distanceX > entity.AttackRange || distanceY > entity.AttackRange) && !asi.IsName("Base Layer.attack") && !asi.IsName("Base Layer.idle_attack"))
                {
                    entity.GetFSM().ChangeState(SoldierChaseState.Instance);
                }
                else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
                {
                    attackDuration -= Time.deltaTime;
                    if (attackDuration <= 0)
                    {
                        attackDuration = 1.5f;
                        entity.TriggerAttackEvent();
                    }

                }
            }
        }*/


    }

    public override void Exit(Soldier entity)
    {
        //Debug.Log("Exit Attack");
    }

}
