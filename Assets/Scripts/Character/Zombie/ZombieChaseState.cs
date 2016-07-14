using UnityEngine;
using System.Collections;

public class ZombieChaseState : State<Zombie>
{

    private static readonly ZombieChaseState instance = new ZombieChaseState();
   
    public static ZombieChaseState Instance
    {
        get
        {
            return instance;
        }
    }


    public override void Enter(Zombie entity)
    {
        entity.ChaseFactor = 2.0f;
        entity.Animator.SetBool("Run", true);
        entity.ShowExclamation();
    }

    public override void Execute(Zombie entity)
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
                entity.GetFSM().ChangeState(ZombiePatrollState.Instance);
            }
            else
            {

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
        else
        {
            entity.Target = null;
            entity.Perspective.Detected = false;
            entity.GetFSM().ChangeState(ZombiePatrollState.Instance);
        }

    }

    public override void Exit(Zombie entity)
    {
        entity.ChaseFactor = 1.0f;
        entity.Controller.Animator.SetBool("Run", false);
    }
}
