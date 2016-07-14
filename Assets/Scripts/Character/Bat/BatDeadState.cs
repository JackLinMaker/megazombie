using UnityEngine;
using System.Collections;

public class BatDeadState : State<Bat>
{
    private static readonly BatDeadState instance = new BatDeadState();

    public static BatDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bat entity)
    {
        entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        entity.Dead();
    }

    public override void Execute(Bat entity)
    {

    }

    public override void Exit(Bat entity)
    {

    }
}
