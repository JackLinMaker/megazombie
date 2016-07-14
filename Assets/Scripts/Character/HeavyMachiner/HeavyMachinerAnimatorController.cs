using UnityEngine;
using System.Collections;

public class HeavyMachinerAnimatorController : MonoBehaviour {

    private HeavyMachiner heavyMachiner;

    void Awake()
    {
        heavyMachiner = this.transform.parent.GetComponent<HeavyMachiner>();

    }

    public void StartAttack()
    {
        //Debug.Log("StartAttack");
        heavyMachiner.Attack();
    }

    public void StopAttack()
    {
        //Debug.Log("StopAttack");
        heavyMachiner.ResetAttackEvent();
    }
}
