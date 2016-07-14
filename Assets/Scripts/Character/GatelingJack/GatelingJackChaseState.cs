using UnityEngine;
using System.Collections;

public class GatelingJackChaseState : State<GatelingJack>
{

    private static readonly GatelingJackChaseState instance = new GatelingJackChaseState();

    public static GatelingJackChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime = 0.0f;

    public override void Enter(GatelingJack entity)
    {

        entity.Animator.SetBool("Run", true);
        entity.Animator.SetBool("Attacking", true);

    }

    public override void Execute(GatelingJack entity)
    {

        if (entity.Target == null)
        {
            entity.GetFSM().ChangeState(GatelingJackSleepState.Instance);
        }
        else
        {
            float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
            float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);



            if (entity.ReachDestination(entity.currentPoint.Current))
            {

                entity.GetFSM().ChangeState(GatelingJackIdleState.Instance);
            }
            else
            {

                entity.Patrol();
                elapsedTime += Time.deltaTime;
                if (elapsedTime > 0.2f)
                {
                    elapsedTime = 0.0f;
                    entity.TriggerAttackEvent();
                }

            }
        }

    }

    public override void Exit(GatelingJack entity)
    {

    }
}
