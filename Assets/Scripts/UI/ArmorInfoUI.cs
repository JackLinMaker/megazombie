using UnityEngine;
using System.Collections;

public class ArmorInfoUI : MonoBehaviour
{
    public UILabel Name;
    public UIButton ActionButton;
    public UISprite ArmorPicture;
    public GameObject FangdanCount;
    public GameObject FangbaozhaCount;
    public GameObject FangfushiCount;
    public void UpdateContent(ArmorItemUI armorUI)
    {
        ResetAllCount();
        ActionButton.onClick.Clear();
        EventDelegate ed = new EventDelegate(BagUI.Instance, "BuyArmor");
        ed.parameters[0] = new EventDelegate.Parameter(armorUI);
        ActionButton.onClick.Add(ed);
    }

    private void ResetAllCount()
    {
        UISprite[] items1 = FangdanCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items1[i].spriteName = "img_weaponinfo_slider2";
        }

        UISprite[] items2 = FangbaozhaCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items2[i].spriteName = "img_weaponinfo_slider2";
        }

        UISprite[] items3 = FangfushiCount.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < 5; i++)
        {
            items3[i].spriteName = "img_weaponinfo_slider2";
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

}
