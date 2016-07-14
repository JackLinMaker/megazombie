using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    public AudioClip PlayerWalkSound;
    public AudioClip PlayerShotSound;
    public AudioClip EnemyShotSound;
    public AudioClip HitSound;
    public AudioClip HitBodySound;
    public AudioClip HitZombieSound;
    public AudioClip ZombieAttack;
    public AudioClip PickGoldSound;
    public AudioClip PickDiamondSound;
    public bool IsDebug = false;



    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        IsDebug = false;
    }

    public void MakePlayerWalkSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(PlayerWalkSound, position, 1f);
        }
    }

    public void MakePlayerShotSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(PlayerShotSound, position, 1f);
        }
    }

    public void MakeEnemyShotSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(EnemyShotSound, position, 1f);
        }
    }

    public void MakeHitSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(HitSound, position, 1f);
        }
    }

    public void MakeHitBodySound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(HitBodySound, position, 1f);
        }
    }

    public void MakeHitZombieSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(HitZombieSound, position, 1f);
        }
    }

    public void MakeZombieAttackSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(ZombieAttack, position, 1f);
        }
    }


    public void MakePickGoldSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(PickGoldSound, position, 1f);
        }
    }

    public void MakePickDiamondSound(Vector3 position)
    {
        if (!IsDebug)
        {
            AudioSource.PlayClipAtPoint(PickDiamondSound, position, 1f);
        }
    }
}
