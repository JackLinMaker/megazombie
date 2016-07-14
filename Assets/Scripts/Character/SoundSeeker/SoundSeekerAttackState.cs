using UnityEngine;
using System.Collections;

public class SoundSeekerAttackState : State<SoundSeeker>
{

    private static readonly SoundSeekerAttackState instance = new SoundSeekerAttackState();

    public static SoundSeekerAttackState Instance
    {
        get
        {
            return instance;
        }
    }

   
    public override void Enter(SoundSeeker entity)
    {
    }

    public override void Execute(SoundSeeker entity)
    {
        entity.AttackDuration -= Time.deltaTime;
        if (entity.AttackDuration <= 0.0)
        {
            entity.AttackDuration = 3.0f;
            entity.TriggerAttackEvent();
        }
       
    }

    public override void Exit(SoundSeeker entity)
    {

    }
}
