using UnityEngine;
using System.Collections;

public class SoldierAnimationController : MonoBehaviour 
{

    private Soldier soldier;

    void Awake()
    {
        soldier = this.transform.parent.GetComponent<Soldier>();

    }

    public void StartAttack()
    {
        //Debug.Log("StartAttack");
        soldier.Attack();
    }

    public void StopAttack()
    {
        //Debug.Log("StopAttack");
        soldier.ResetAttackEvent();
    }

    public void StartHurt()
    {
        Debug.Log("StartHurt");

    }

    public void StopHurt()
    {
        //Debug.Log("StopHurt");
        soldier.Animator.SetBool("Hurt", false);
    }

    public void MakeDeadSound()
    {
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
    }
   
}
