using UnityEngine;
using System.Collections;

public class WeaponItemUI : MonoBehaviour
{
    public WeaponInfo WeaponInfo { get; set; }
    public UISprite WeaponPicture;
    public UILabel WeaponPriceLabel;
    public UISprite WeaponPricePicture;
    public GameObject EquipTip;
    private BagUI BagUI;
    // Use this for initialization
    void Start()
    {
        BagUI = GameObject.FindObjectOfType<BagUI>();
        EventDelegate eventDelegate = new EventDelegate(BagUI, "WeaponInfoOpen");
        eventDelegate.parameters[0] = new EventDelegate.Parameter(WeaponInfo);
        this.GetComponent<UIButton>().onClick.Add(eventDelegate);
        if (DataManager.Instance.PInfo.BuyedWeapons.Contains(WeaponInfo.Id))
        {
            WeaponPricePicture.enabled = false;
            WeaponPriceLabel.enabled = false;
        }
        else
        {
            WeaponPricePicture.enabled = true;
            WeaponPriceLabel.enabled = true;
            if (WeaponInfo.Basic != 0)
            {
                WeaponPriceLabel.text = WeaponInfo.Basic.ToString();
                WeaponPricePicture.spriteName = "img_Basic";
            }
            else if (WeaponInfo.Advanced != 0)
            {
                WeaponPriceLabel.text = WeaponInfo.Advanced.ToString();
                WeaponPricePicture.spriteName = "img_Advanced";
            }
        }

    }


}
