using UnityEngine;
using System.Collections;

public class SoldierPauseState : State<Soldier> 
{

    private static readonly SoldierPauseState instance = new SoldierPauseState();

    public static SoldierPauseState Instance
    {
        get
        {
            return instance;
        }
    }


    public override void Enter(Soldier entity)
    {
        
        entity.Animator.SetFloat("Speed", 0);
    }

    public override void Execute(Soldier entity)
    {
        if (entity.GetComponent<Perspective>().Detected)
        {
            entity.GetFSM().ChangeState(SoldierChaseState.Instance);
        }
        else
        {
            if (entity.Pause())
            {
                entity.GetFSM().ChangeState(SoldierPatrollState.Instance);
            }
        }
        
       
    }

    public override void Exit(Soldier entity)
    {
        //Debug.Log("Exit Pause");
    }
}
