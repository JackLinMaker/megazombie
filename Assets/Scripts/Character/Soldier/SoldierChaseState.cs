using UnityEngine;
using System.Collections;

public class SoldierChaseState : State<Soldier> 
{
    private static readonly SoldierChaseState instance = new SoldierChaseState();

    public static SoldierChaseState Instance
    {
        get
        {
            return instance;
        }
    }

   
    public override void Enter(Soldier entity)
    {
        entity.Animator.SetFloat("Speed", 1);
        entity.ShowExclamation();
        
    }

    public override void Execute(Soldier entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
            float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

            AnimatorStateInfo asi = entity.Animator.GetCurrentAnimatorStateInfo(0);
            if ((entity.Target.position.x < entity.PatrolPath.GetMinXPoint().position.x - entity.AttackRange
                || entity.Target.position.x > entity.PatrolPath.GetMaxXPoint().position.x + entity.AttackRange)
                && !asi.IsName("Base Layer.attack") && !asi.IsName("Base Layer.idle_attack"))
            {
                entity.Target = null;
                entity.GetComponent<Perspective>().Detected = false;
                if (entity.PatrolPath != null)
                {
                    entity.GetFSM().ChangeState(SoldierPatrollState.Instance);
                }
            }

            if (distanceX > entity.AttackRange
                && entity.transform.position.x > entity.PatrolPath.GetMinXPoint().position.x
                && entity.transform.position.x < entity.PatrolPath.GetMaxXPoint().position.x
                )
            {

                entity.Chase();
            }
            else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
            {
                entity.Controller.SetHorizontalForce(0.0f);
                entity.GetFSM().ChangeState(SoldierAttackState.Instance);
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
    }

    public override void Exit(Soldier entity)
    {
        Debug.Log("Exit Chase");
       
        
    }
}
