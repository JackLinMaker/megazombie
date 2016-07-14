using UnityEngine;
using System.Collections;

public class FireBatChaseState : State<FireBat>
{

    private static readonly FireBatChaseState instance = new FireBatChaseState();

    public static FireBatChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(FireBat entity)
    {

    }

    public override void Execute(FireBat entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distance = Vector3.Distance(entity.Target.transform.position, entity.transform.position);

            if (distance >= entity.ChaseRange)
            {
                entity.Target = null;
                entity.Perspective.Detected = false;
                entity.GetFSM().ChangeState(FireBatPatrolState.Instance);

            }
            else if (distance < entity.ChaseRange)
            {
                entity.AttackDuration -= Time.deltaTime;
                if (entity.AttackDuration > 0)
                {
                    entity.Fly();
                }
                else if(entity.AttackDuration <= 0.0)
                {
                    entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    if (entity.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.fly"))
                    {
                        entity.Animator.SetBool("Attacking", true);
                    }
                   
                }
            }

        }
        else
        {
            entity.Target = null;
            entity.Perspective.Detected = false;
            entity.GetFSM().ChangeState(FireBatPatrolState.Instance);
        }
    }

    public override void Exit(FireBat entity)
    {

    }
}
