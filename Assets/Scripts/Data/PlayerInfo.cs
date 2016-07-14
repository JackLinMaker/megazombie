using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerInfo
{
    public int Gold;
    public int MainLevel;
    public int SubLevel;
    public List<int> BuyedWeapons;
    public WeaponInfo RWeapon;
    public WeaponInfo MWeapon;
}
