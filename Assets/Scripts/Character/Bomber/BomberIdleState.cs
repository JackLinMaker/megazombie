using UnityEngine;
using System.Collections;

public class BomberIdleState : State<Bomber>
{
    private static readonly BomberIdleState instance = new BomberIdleState();

    public static BomberIdleState Instance
    {
        get
        {
            return instance;
        }
    }


    private float elapsedTime = 0.0f;

    public override void Enter(Bomber entity)
    {
        entity.Controller.SetHorizontalForce(0.0f);

    }

    public override void Execute(Bomber entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.ShowQuestion();
            entity.GetFSM().ChangeState(BomberAttackState.Instance);
        }
    }

    public override void Exit(Bomber entity)
    {

    }
	
}
