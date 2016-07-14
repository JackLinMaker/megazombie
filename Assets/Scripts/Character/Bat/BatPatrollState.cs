using UnityEngine;
using System.Collections;

public class BatPatrollState : State<Bat> 
{
    private static readonly BatPatrollState instance = new BatPatrollState();

    public static BatPatrollState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bat entity)
    {
        entity.Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Execute(Bat entity)
    {

         Player player = entity.Target.transform.GetComponent<Player>();
         if (player != null && player.CurrentHealth > 0)
         {
             float distance = Vector3.Distance(entity.Target.transform.position, entity.transform.position);
             if (distance >= entity.ChaseRange)
             {
                 if (entity.PatrolPath != null)
                 {
                   
                     
                     if (Vector3.Distance(entity.currentPoint.Current.position, entity.transform.position) < 0.1f)
                     {
                         entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                         entity.GetFSM().ChangeState(BatPauseState.Instance);
                     }
                     else
                     {
                         entity.Fly();
                     }

                 }
             }
             else
             {
                 entity.ShowQuestion();
                 entity.GetFSM().ChangeState(BatChaseState.Instance);
             }
         }
    }

    public override void Exit(Bat entity)
    { 
    
    }
	
}
