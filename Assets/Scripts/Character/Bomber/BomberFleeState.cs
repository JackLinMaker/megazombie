using UnityEngine;
using System.Collections;

public class BomberFleeState : State<Bomber>
{

    private static readonly BomberFleeState instance = new BomberFleeState();

    public static BomberFleeState Instance
    {
        get
        {
            return instance;
        }
    }


    private float fleeDistance = 15.0f;

    public override void Enter(Bomber entity)
    {
        Debug.Log("Enter Flee");
        entity.Animator.SetBool("Attacking", false);
        entity.Animator.SetBool("Run", true);
        entity.ShowExclamation();
    }

    public override void Execute(Bomber entity)
    {
        if (entity.Target == null)
        {
            entity.GetFSM().ChangeState(BomberIdleState.Instance);
            entity.GetComponent<Perspective>().Detected = false;
        }
        else
        {
            if ((entity.Target.GetComponent<Player>() != null && entity.Target.GetComponent<Player>().CurrentHealth <= 0) || (entity.Target.GetComponent<BaseEntity>() != null && entity.Target.GetComponent<BaseEntity>().CurrentHealth <= 0))
            {
                entity.Target = null;
                entity.GetFSM().ChangeState(BomberIdleState.Instance);
                entity.GetComponent<Perspective>().Detected = false;
            }
            else
            {
                float distanceX = Mathf.Abs(entity.transform.position.x - entity.Target.position.x);
                float sign = Mathf.Sign(entity.transform.position.x - entity.Target.position.x);

                if (distanceX < fleeDistance)
                {
                    entity.Flee(sign);
                }
                else if (distanceX >= fleeDistance)
                {
                    entity.Animator.SetBool("Run", false);
                    entity.Controller.SetHorizontalForce(0.0f);
                    entity.GetFSM().ChangeState(BomberIdleState.Instance);

                }
            }
          
        }
        
    }

    public override void Exit(Bomber entity)
    {

    }
}
