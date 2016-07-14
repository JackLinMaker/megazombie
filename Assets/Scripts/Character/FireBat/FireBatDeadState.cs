using UnityEngine;
using System.Collections;

public class FireBatDeadState :  State<FireBat> 
{

    private static readonly FireBatDeadState instance = new FireBatDeadState();

    public static FireBatDeadState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(FireBat entity)
    {
        entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        entity.Dead();
    }

    public override void Execute(FireBat entity)
    {
       
    }

    public override void Exit(FireBat entity)
    {
        
    }
}
