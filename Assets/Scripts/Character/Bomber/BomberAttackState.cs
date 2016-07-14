using UnityEngine;
using System.Collections;

public class BomberAttackState : State<Bomber>
{

    private static readonly BomberAttackState instance = new BomberAttackState();
    private float attackDuration = 0.0f;

    public static BomberAttackState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bomber entity)
    {
       
    }

    public override void Execute(Bomber entity)
    {
         Player player = entity.Target.transform.GetComponent<Player>();
         if (player != null && player.CurrentHealth > 0)
         {
             float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
             float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

             AnimatorStateInfo stateInfo = entity.Animator.GetCurrentAnimatorStateInfo(0);

             if (distanceX > entity.AttackRange && stateInfo.nameHash != entity.AttackStateHash)
             {
                 entity.GetFSM().ChangeState(BomberIdleState.Instance);
                 entity.GetComponent<Perspective>().Detected = false;
             }
             else if (distanceX <= entity.AttackRange && distanceY <= entity.AttackRange)
             {
                 attackDuration -= Time.deltaTime;
                 if (attackDuration <= 0)
                 {
                     attackDuration = 2.5f;
                     entity.TriggerAttackEvent();
                 }
             }
         }
         else
         {
             entity.Target = null;
             entity.Perspective.Detected = false;
             entity.GetFSM().ChangeState(BomberIdleState.Instance);
         }

     
    }

    public override void Exit(Bomber entity)
    {
        
    }
}
