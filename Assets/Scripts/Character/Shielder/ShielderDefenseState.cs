using UnityEngine;
using System.Collections;

public class ShielderDefenseState : State<Shielder>
{

    private static readonly ShielderDefenseState instance = new ShielderDefenseState();

    public static ShielderDefenseState Instance
    {
        get
        {
            return instance;
        }
    }

    private float elapsedTime = 0.0f;
    private int count = 3;
    private float attacktime = 0;
    public override void Enter(Shielder entity)
    {
        attacktime = 2f;
        if (entity.GetFSM().PreviousState() == ShielderPatrolState.Instance)
        {
            entity.Controller.SetHorizontalForce(0.0f);
            entity.Animator.SetBool("Walk", false);
        }
        else if (entity.GetFSM().PreviousState() == ShielderChaseState.Instance)
        {
            entity.Controller.SetHorizontalForce(0.0f);
            entity.Animator.SetBool("Run", false);
        }

        //entity.FaceToTarget();
        entity.Animator.SetBool("Defense", true);
        entity.ShowShield();

    }

    public override void Execute(Shielder entity)
    {
        Player player = entity.Target.transform.GetComponent<Player>();
        if (player != null && player.CurrentHealth > 0)
        {
            float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
            float distanceY = Mathf.Abs(entity.transform.position.y - entity.Target.position.y);

            if (distanceX > entity.AttackRange)
            {
                entity.Target = null;
                entity.GetComponent<Perspective>().Detected = false;
                if (entity.PatrolPath != null)
                {
                    entity.GetFSM().ChangeState(ShielderPatrolState.Instance);
                }
                else
                {
                    entity.GetFSM().ChangeState(ShielderIdleState.Instance);
                }

            }
            else
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 4f)
                {
                    entity.Animator.SetBool("Defense", false);
                    entity.HideShield();
                    if (count > 0)
                    {
                        AnimatorStateInfo asi = entity.Animator.GetCurrentAnimatorStateInfo(0);
                        if (!asi.IsName("Base Layer.TransAttack"))
                        {
                            attacktime += Time.deltaTime;
                        }

                        if (attacktime > 2f)
                        {
                            if (asi.IsName("Base Layer.Attacking"))
                            {
                                entity.FastAttack();
                            }
                            else
                            {
                                entity.TriggerAttack();
                            }
                            attacktime = 0;
                            count--;
                        }
                    }
                    else
                    {
                        elapsedTime = 0.0f;
                        attacktime = 2f;
                        count = 3;
                        entity.Animator.SetBool("Defense", true);
                        entity.ShowShield();
                    }

                }
            }
        }
        else
        {
            entity.Target = null;
            entity.Perspective.Detected = false;
            entity.GetFSM().ChangeState(ShielderIdleState.Instance);
        }
          
    }

    public override void Exit(Shielder entity)
    {

        entity.Animator.SetBool("Defense", false);
        elapsedTime = 0.0f;
    }
}
