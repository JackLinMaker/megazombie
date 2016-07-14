using UnityEngine;
using System.Collections;

public class FireBatPauseState : State<FireBat>
{
    private static readonly FireBatPauseState instance = new FireBatPauseState();

    public static FireBatPauseState Instance
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
       
    }

    public override void Exit(FireBat entity)
    {
       
    }
}
