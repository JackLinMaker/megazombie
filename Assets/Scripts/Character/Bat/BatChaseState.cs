using UnityEngine;
using System.Collections;

public class BatChaseState : State<Bat>  
{
    private static readonly BatChaseState instance = new BatChaseState();

    public static BatChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bat entity)
    {
        entity.ShowExclamation();
    }

    public override void Execute(Bat entity)
    {
         Player player = entity.Target.transform.GetComponent<Player>();
         if (player != null && player.CurrentHealth > 0)
         {
             float distance = Vector3.Distance(entity.Target.transform.position, entity.transform.position);
          
             if (distance >= entity.ChaseRange)
             {
                 entity.Target = null;
                 entity.Perspective.Detected = false;
                 entity.GetFSM().ChangeState(BatPatrollState.Instance);

             }
             else if (distance < entity.ChaseRange && distance >= entity.AttackRange)
             {
                 Vector2 diff = entity.Target.transform.position - entity.transform.position;
                 entity.Flip(diff.x);
                 entity.GetComponent<Rigidbody2D>().velocity = diff.normalized * entity.SpeedAccelerationInAir;
             }
             else if(distance < entity.AttackRange)
             {
                 entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             }
         }
         else
         {
             entity.Target = null;
             entity.Perspective.Detected = false;
             entity.GetFSM().ChangeState(BatPatrollState.Instance);
         }
    }

    public override void Exit(Bat entity)
    {

    }
	
}
