using UnityEngine;
using System.Collections;

public class FireBatAnimationController : MonoBehaviour {

    private FireBat fireBat;

    void Awake()
    {
        fireBat = this.transform.parent.GetComponent<FireBat>();
    }

    public void StartAttack()
    {
        fireBat.Attack();
    }
}
