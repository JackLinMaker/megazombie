using UnityEngine;
using System.Collections;

public class WeaponInfoUI : MonoBehaviour
{
    public UILabel WeaponName;
    public UIButton ActionButton;
    public UILabel ActionContent;
    public UISprite WeaponPicture;
    public UILabel BulletCount;
    public GameObject RangeCount;
    public GameObject RateCount;
    public GameObject DamageCount;
    private WeaponInfo currentWeaponInfo;



    public void UpdateContent(WeaponInfo item)
    {
        ResetAllCount();
        WeaponName.text = item.Name;
        currentWeaponInfo = item;
        WeaponPicture.spriteName = item.Name;
        BulletCount.text = item.BulletCount.ToString();
        UpdateCount(item);

        ActionButton.onClick.Clear();
        if (DataManager.Instance.PInfo.BuyedWeapons.Contains(currentWeaponInfo.Id))
        {

            ActionContent.text = "装   备";
            EventDelegate ed = new EventDelegate(BagUI.Instance, "EquipWeapon");
            ed.parameters[0] = new EventDelegate.Parameter(currentWeaponInfo);
            ActionButton.onClick.Add(ed);
        }
        else
        {
            ActionContent.text = "购   买";
            EventDelegate ed = new EventDelegate(BagUI.Instance, "BuyWeapon");
            ed.parameters[0] = new EventDelegate.Parameter(currentWeaponInfo);
            ActionButton.onClick.Add(ed);
        }

    }

    private void SetValue(GameObject count, int value)
    {
        UISprite[] items = count.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < value; i++)
        {
            items[i].spriteName = "img_weaponinfo_slider1";
        }
    }

    private void ResetAllCount()
    {
        UISprite[] items1 = RangeCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items1[i].spriteName = "img_weaponinfo_slider2";
        }

        UISprite[] items2 = RateCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items2[i].spriteName = "img_weaponinfo_slider2";
        }

        UISprite[] items3 = DamageCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items3[i].spriteName = "img_weaponinfo_slider2";
        }
    }

    private void UpdateCount(WeaponInfo item)
    {
        if (item.Range > 12)
        {
            SetValue(RangeCount, 5);
        }
        else if (item.Range > 10 && item.Range <= 12)
        {
            SetValue(RangeCount, 4);
        }
        else if (item.Range > 7 && item.Range <= 10)
        {
            SetValue(RangeCount, 3);
        }
        else if (item.Range > 3 && item.Range <= 7)
        {
            SetValue(RangeCount, 2);
        }
        else if (item.Range > 0 && item.Range <= 3)
        {
            SetValue(RangeCount, 1);
        }

        if (item.Rate > 1.0f)
        {
            SetValue(RateCount, 1);
        }
        else if (item.Rate > 0.7f && item.Rate <= 1.0f)
        {
            SetValue(RateCount, 2);
        }
        else if (item.Rate > 0.4f && item.Rate <= 0.7f)
        {
            SetValue(RateCount, 3);
        }
        else if (item.Rate > 0.04f && item.Rate <= 0.4f)
        {
            SetValue(RateCount, 4);
        }
        else if (item.Rate > 0 && item.Rate <= 0.04f)
        {
            SetValue(RateCount, 5);
        }

        if (item.Damage > 150)
        {
            SetValue(DamageCount, 5);
        }
        else if (item.Damage > 100 && item.Damage <= 150)
        {
            SetValue(DamageCount, 4);
        }
        else if (item.Damage > 60 && item.Damage <= 100)
        {
            SetValue(DamageCount, 3);
        }
        else if (item.Damage > 20 && item.Damage <= 60)
        {
            SetValue(DamageCount, 2);
        }
        else if (item.Damage > 0 && item.Damage <= 20)
        {
            SetValue(DamageCount, 1);
        }
    }

}
