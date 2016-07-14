using UnityEngine;
using System.Collections;

public class SoundSeekerDeadState : State<SoundSeeker>
{

    private static readonly SoundSeekerDeadState instance = new SoundSeekerDeadState();

    public static SoundSeekerDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(SoundSeeker entity)
    {
        Debug.Log("Enter Head Dead");
        entity.Controller.SetHorizontalForce(0.0f);
        entity.Dead();
    }

    public override void Execute(SoundSeeker entity)
    {


    }

    public override void Exit(SoundSeeker entity)
    {
        Debug.Log("Exit Head Dead");
    }
}
