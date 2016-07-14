using UnityEngine;
using System.Collections;

public class SoundSeekerAnimationController : MonoBehaviour 
{
    private SoundSeeker soundSeeker;

    void Awake()
    {
        soundSeeker = this.transform.parent.GetComponent<SoundSeeker>();
    }

    public void StartAttack()
    {
        soundSeeker.Attack();
    }

    public void StopAttack()
    {
        
        soundSeeker.ResetAttackEvent();
    }

    public void MakeDeadSound()
    {
        SoundManager.Instance.MakeHitBodySound(this.transform.position);
    }
}
