using UnityEngine;
using System.Collections;

public class MissileCarAnimatorController : MonoBehaviour {

    private MissileCar missileCar;

    void Awake()
    {
        missileCar = this.transform.parent.GetComponent<MissileCar>();

    }

    public void StartAttackLeft1()
    {
        missileCar.AttackLeft1();
    }

    public void StartAttackLeft2()
    {
        missileCar.AttackLeft2();
    }

    public void StartAttackRight1()
    {
        missileCar.AttackRight1();
    }

    public void StartAttackRight2()
    {
        missileCar.AttackRight2();
    }

    public void StopAttack()
    {
        //Debug.Log("StopAttack");
        missileCar.ResetAttackEvent();
    }
}
