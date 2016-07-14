using UnityEngine;
using System.Collections;

public class SergentDanAnimatorController : MonoBehaviour 
{

    private SergentDan sergent;

    void Awake()
    {
        sergent = this.transform.parent.GetComponent<SergentDan>();
    }

    public void StartCRAttack()
    {
        sergent.CRAttack();
    }

    public void StopCRAttack()
    {
        sergent.FinishCRAttack();
    }

    public void Fly()
    {
        sergent.Fly();
    }

    public void EarthQuake()
    {
        sergent.EarthQuake();
    }

    public void StartLRAttack()
    {
        sergent.LRAttack();
    }

    public void StartChase()
    {
        sergent.Chase();
    }

    public void MakeDeadSound()
    {
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
    }
}
