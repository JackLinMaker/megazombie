using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponInfo 
{

    public enum WType
    {
        ShotGun,
        HandGun
    };


    public int Id;

    public string Name;

    public int WeaponType;

    public double Damage;

    public double Range;

    public double Rate;

    public int BulletCount;

    public double BulletSpeed;

    public int IsPurchased;

    public int Basic;

    public int Advanced;

    public double HitForceX;

    public double HitForceY;

    public int IsAutomatic;
    
}
