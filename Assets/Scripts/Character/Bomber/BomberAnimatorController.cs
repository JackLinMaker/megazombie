using UnityEngine;
using System.Collections;

public class BomberAnimatorController : MonoBehaviour {

    private Bomber bomber;

    void Awake()
    {
        bomber = this.transform.parent.GetComponent<Bomber>();
    }

    public void StartAttack()
    {
        //Debug.Log("Bomber StartAttack");
        bomber.Attack();
    }

    public void StopAttack()
    {
        //Debug.Log("Bomber StopAttack");
        bomber.ResetAttackEvent();
    }

    public void MakeDeadSound()
    {
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
    }
}
    