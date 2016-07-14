using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagUI : MonoBehaviour
{
    // 武器详细信息页面
    public GameObject WeaponInfoGrid;

    public GameObject ArmorInfoGrid;

    // 武器项预设
    public GameObject WeaponItem;

    // 近战武器列表
    public GameObject HandGunWeaponGrid;

    // 远程武器列表
    public GameObject ShotGunWeaponGrid;

    public UILabel BasicCount;

    public UILabel AdvancedCount;

    public GameObject WarningWindow;

    private Dictionary<int, GameObject> HandWeaponItems = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> ShotWeaponItems = new Dictionary<int, GameObject>();

    private static BagUI _instance;

    //This is the public reference that other classes will use
    public static BagUI Instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<BagUI>();
            return _instance;
        }
    }

    void Start()
    {
        //LoadWeapon();
    }

    void Update()
    {

    }

    public void LoadWeapon()
    {
        int i = 0;
        int j = 0;
        // 加载武器数据
        foreach (KeyValuePair<int, WeaponInfo> weapon in DataManager.Instance.Weapons)
        {
            GameObject item = Instantiate(WeaponItem) as GameObject;

            item.GetComponent<WeaponItemUI>().WeaponInfo = weapon.Value;
            item.GetComponent<WeaponItemUI>().WeaponPicture.spriteName = weapon.Value.Name;
            if (weapon.Value.WeaponType == (int)WeaponInfo.WType.HandGun)
            {
                item.transform.parent = HandGunWeaponGrid.transform;
                item.transform.localPosition = new Vector3(i * 175, 0, 0);
                HandWeaponItems.Add(weapon.Value.Id, item);
                i++;
            }
            else
            {
                item.transform.parent = ShotGunWeaponGrid.transform;
                item.transform.localPosition = new Vector3(j * 175, 0, 0);
                ShotWeaponItems.Add(weapon.Value.Id, item);
                j++;
            }
            item.transform.localScale = new Vector3(1, 1, 1);

        }

        EquipWeapon(DataManager.Instance.PInfo.RWeapon);
        EquipWeapon(DataManager.Instance.PInfo.MWeapon);
    }

    // 换武器
    public void EquipWeapon(WeaponInfo weapon)
    {
        if (weapon.WeaponType == (int)WeaponInfo.WType.HandGun)
        {
            HandWeaponItems[DataManager.Instance.PInfo.MWeapon.Id].GetComponent<WeaponItemUI>().EquipTip.SetActive(false);
            DataManager.Instance.PInfo.MWeapon = weapon;
            HandWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().EquipTip.SetActive(true);
        }
        else
        {
            ShotWeaponItems[DataManager.Instance.PInfo.RWeapon.Id].GetComponent<WeaponItemUI>().EquipTip.SetActive(false);
            DataManager.Instance.PInfo.RWeapon = weapon;
            ShotWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().EquipTip.SetActive(true);
        }
        WeaponInfoClose();
    }

    public void BuyWeapon(WeaponInfo weapon)
    {
        /*if (DataManager.Instance.PInfo.Basic < weapon.Basic || DataManager.Instance.PInfo.Advanced < weapon.Advanced)
        {
            WeaponInfoClose();

            // 弹出警告
            WarningWindow.GetComponent<WarningUI>().Show("零件不足!");

            return;
        }
        DataManager.Instance.PInfo.Basic -= weapon.Basic;
        DataManager.Instance.PInfo.Advanced -= weapon.Advanced;
        DataManager.Instance.PInfo.BuyedWeapons.Add(weapon.Id);
        DataManager.Instance.SavePlayerInfo();

        if (weapon.WeaponType == 0)
        {
            ShotWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponInfo = DataManager.Instance.Weapons[weapon.Id];
            ShotWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponPriceLabel.enabled = false;
            ShotWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponPricePicture.enabled = false;
        }
        else if (weapon.WeaponType == 1)
        {
            HandWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponInfo = DataManager.Instance.Weapons[weapon.Id];
            HandWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponPriceLabel.enabled = false;
            HandWeaponItems[weapon.Id].GetComponent<WeaponItemUI>().WeaponPricePicture.enabled = false;
        }

        UpdatePlayerInfo();

        EquipWeapon(weapon);*/
    }

    public void BuyArmor(ArmorItemUI armorUI)
    {
        /*if (DataManager.Instance.PInfo.Basic <armorUI.Armor.Basic)
        {
            ArmorInfoClose();

            // 弹出警告
            WarningWindow.GetComponent<WarningUI>().Show("零件不足!");

            return;
        }

        DataManager.Instance.PInfo.Basic -= (int)armorUI.Armor.Basic;
        DataManager.Instance.SavePlayerInfo();
        armorUI.BuyedState();
        DataManager.Instance.CurrentArmor = armorUI.Armor;
        UpdatePlayerInfo();
        ArmorInfoClose();*/
    }

    public void CloseWarningWindow()
    {
        WarningWindow.SetActive(false);
    }

    public void UpdatePlayerInfo()
    {
        //BasicCount.text = DataManager.Instance.PInfo.Basic.ToString();
        //AdvancedCount.text = DataManager.Instance.PInfo.Advanced.ToString();
    }

    public void WeaponInfoOpen(WeaponInfo item)
    {
        WeaponInfoGrid.SetActive(true);
        WeaponInfoGrid.GetComponent<WeaponInfoUI>().UpdateContent(item);
    }

    public void ArmorInfoOpen(ArmorItemUI armorUI)
    {
        if (armorUI.IsBuy == true)
        {
            return;
        }
        ArmorInfoGrid.SetActive(true);
        ArmorInfoGrid.GetComponent<ArmorInfoUI>().UpdateContent(armorUI);
       
    }

    public void WeaponInfoClose()
    {
        WeaponInfoGrid.SetActive(false);
    }

    public void ArmorInfoClose()
    {
        ArmorInfoGrid.SetActive(false);
    }
}
