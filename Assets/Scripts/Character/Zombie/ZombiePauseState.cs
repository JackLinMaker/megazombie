using UnityEngine;
using System.Collections;

public class ZombiePauseState : State<Zombie> 
{
    private static readonly ZombiePauseState instance = new ZombiePauseState();

    public static ZombiePauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Zombie entity)
    {
        
        entity.Animator.SetBool("Walk", false);
    }

    public override void Execute(Zombie entity)
    {
        if (entity.Pause())
        {
            entity.GetFSM().ChangeState(ZombiePatrollState.Instance);
        }
    }

    public override void Exit(Zombie entity)
    {

      
    }
	
}
