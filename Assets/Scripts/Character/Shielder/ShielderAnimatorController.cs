using UnityEngine;
using System.Collections;

public class ShielderAnimatorController : MonoBehaviour {

    private Shielder shielder;

    void Awake()
    {
        shielder = this.transform.parent.GetComponent<Shielder>();
    }

    public void StartAttack()
    {
        shielder.Attack();
    }

    public void StopAttack()
    {
        shielder.ResetAttackEvent();
    }

    public void MakeDeadSound()
    {
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
    }
}
