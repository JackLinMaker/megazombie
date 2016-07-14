using UnityEngine;
using System.Collections;

public class ArmorItemUI : MonoBehaviour
{
    public UISprite ArmorPicture;
    public UILabel ArmorPriceLabel;
    public UISprite ArmorPricePicture;
    public GameObject EquipTip;
    public bool IsBuy;
    private BagUI BagUI;
    void Start()
    {
        UnBuyState();
        BagUI = GameObject.FindObjectOfType<BagUI>();
        EventDelegate eventDelegate = new EventDelegate(BagUI, "ArmorInfoOpen");
        eventDelegate.parameters[0] = new EventDelegate.Parameter(this);
        this.GetComponent<UIButton>().onClick.Add(eventDelegate);
    }

    public void BuyedState()
    {
        ArmorPriceLabel.gameObject.SetActive(false);
        ArmorPricePicture.gameObject.SetActive(false);
        EquipTip.SetActive(true);
        IsBuy = true;
    }

    public void UnBuyState()
    {
        ArmorPriceLabel.gameObject.SetActive(true);
        ArmorPricePicture.gameObject.SetActive(true);
        EquipTip.SetActive(false);
        IsBuy = false;
    }
}
