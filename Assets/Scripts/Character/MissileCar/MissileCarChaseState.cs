using UnityEngine;
using System.Collections;

public class MissileCarChaseState : State<MissileCar>
{
    private static readonly MissileCarChaseState instance = new MissileCarChaseState();

    public static MissileCarChaseState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(MissileCar entity)
    {

    }

    public override void Execute(MissileCar entity)
    {

    }

    public override void Exit(MissileCar entity)
    {

    }
}
