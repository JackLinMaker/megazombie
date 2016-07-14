using UnityEngine;
using System.Collections;

public class BatPauseState : State<Bat>
{
    private static readonly BatPauseState instance = new BatPauseState();

    public static BatPauseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bat entity)
    {
        
    }

    public override void Execute(Bat entity)
    {
        if (entity.Pause())
        {
            entity.GetFSM().ChangeState(BatPatrollState.Instance);
        }
    }

    public override void Exit(Bat entity)
    {


    }
	
}
