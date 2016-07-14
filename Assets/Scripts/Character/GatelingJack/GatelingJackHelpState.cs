using UnityEngine;
using System.Collections;

public class GatelingJackHelpState : State<GatelingJack>
{

    private static readonly GatelingJackHelpState instance = new GatelingJackHelpState();

    public static GatelingJackHelpState Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(GatelingJack entity)
    {


    }

    public override void Execute(GatelingJack entity)
    {

    }

    public override void Exit(GatelingJack entity)
    {

    }
}
