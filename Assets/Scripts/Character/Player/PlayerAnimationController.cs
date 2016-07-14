using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = this.transform.parent.GetComponent<Player>();

    }

    // 在attack_gun 的射击前一帧
    public void StartAttack()
    {
        player.ShootBullet();
    }

    // 在attack_gun 的射击后一帧
    public void StopAttack()
    {
        player.ResetShootState();
    }

    public void MakePlayerWalkSound()
    {
        SoundManager.Instance.MakePlayerWalkSound(this.transform.position);
    }

    public void MakePlayerShootSound()
    {
        SoundManager.Instance.MakePlayerShotSound(this.transform.position);
    }

}
