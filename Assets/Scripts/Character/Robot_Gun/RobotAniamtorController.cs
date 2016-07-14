using UnityEngine;
using System.Collections;

public class RobotAniamtorController : MonoBehaviour 
{
    private RobotGun robotGun;

    void Awake()
    {
        robotGun = this.transform.parent.GetComponent<RobotGun>();

    }

    public void StartAttackLeft()
    {
        robotGun.AttackLeft();
    }

    public void StartAttackRight()
    {
        robotGun.AttackRight();
    }
}
