using UnityEngine;
using System.Collections;

public class HeavyMachinerAttackState : State<HeavyMachiner>
{

    private static readonly HeavyMachinerAttackState instance = new HeavyMachinerAttackState();
    private float attackDuration;
    private float Timer = 0;
    private float idleDuration;

    public static HeavyMachinerAttackState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(HeavyMachiner entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Animator.SetBool("Walk", false);
        
        attackDuration = 1f;
        idleDuration = -4f;
        Timer = -3.5f;
    }

    public override void Execute(HeavyMachiner entity)
    {
          Player player = entity.Target.transform.GetComponent<Player>();
          if (player != null && player.CurrentHealth > 0)
          {
              Timer -= Time.deltaTime;
              float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
              float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

              if (distanceX > entity.AttackRange)
              {
                  entity.Target = null;
                  entity.GetComponent<Perspective>().Detected = false;
                  entity.GetFSM().ChangeState(HeavyMachinerIdleState.Instance);
                  entity.ResetAttackEvent();
              }
              else if (distanceX <= entity.AttackRange && Timer >= 0)
              {
                  entity.TriggerAttackEvent();
              }
              else if (Timer < 0 && Timer >= idleDuration)
              {
                  entity.Animator.SetBool("Attacking", false);
                  entity.ResetAttackEvent();
              }
              else if (Timer < idleDuration)
              {
                  Timer = attackDuration;
              }
          }
          else
          {
              entity.Target = null;
              entity.Perspective.Detected = false;
              entity.GetFSM().ChangeState(HeavyMachinerIdleState.Instance);
          }

       
    }

    public override void Exit(HeavyMachiner entity)
    {

    }
}
