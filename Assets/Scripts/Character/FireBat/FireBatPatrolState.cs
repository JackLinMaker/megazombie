using UnityEngine;
using System.Collections;

public class FireBatPatrolState : State<FireBat>
{

    private static readonly FireBatPatrolState instance = new FireBatPatrolState();

    public static FireBatPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(FireBat entity)
    {
        entity.Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Execute(FireBat entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distance = Vector3.Distance(entity.Target.transform.position, entity.transform.position);
            if (distance >= entity.ChaseRange)
            {
                float dis = Vector3.Distance(entity.transform.position, entity.StartPoint);
                if (dis > 0.1f)
                {
                    entity.Back();
                }
                else
                {
                    entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }
            else
            {
                entity.ShowQuestion();
                entity.GetFSM().ChangeState(FireBatChaseState.Instance);
            }
        }
    }

    public override void Exit(FireBat entity)
    {

    }
}
