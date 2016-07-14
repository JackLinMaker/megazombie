using UnityEngine;
using System.Collections;

public class HUDPanel : MonoBehaviour 
{
    // Control Button
    public GameObject BtnLeft;
    public GameObject BtnRight;
    public GameObject BtnJump;
    public GameObject BtnAttack;
    public GameObject BtnPause;


    // Attributes
    public UIProgressBar HealthBar;
    public UILabel GoldLabel;
    public GameObject Hostages;


    private Player player;
    private float maxHealth;

    void Awake()
    {
        HealthBar.GetComponent<UIProgressBar>().value = 1.0f;
        player = GameObject.FindObjectOfType<Player>();
        
        addControl();

    }

    void Start()
    {
        maxHealth = player.GetComponent<Player>().CurrentHealth;
        UpdateGold(DataManager.Instance.PInfo.Gold);
    }

    private void addControl()
    {
        BtnLeft.GetComponent<UIEventTrigger>().onPress.Add(new EventDelegate(player, "MoveBackward"));
        BtnLeft.GetComponent<UIEventTrigger>().onDragOver.Add(new EventDelegate(player, "MoveBackward"));
        BtnLeft.GetComponent<UIEventTrigger>().onRelease.Add(new EventDelegate(player, "Stop"));
        BtnLeft.GetComponent<UIEventTrigger>().onDragOut.Add(new EventDelegate(player, "Stop"));

        BtnRight.GetComponent<UIEventTrigger>().onPress.Add(new EventDelegate(player, "MoveForward"));
        BtnRight.GetComponent<UIEventTrigger>().onDragOver.Add(new EventDelegate(player, "MoveForward"));
        BtnRight.GetComponent<UIEventTrigger>().onRelease.Add(new EventDelegate(player, "Stop"));
        BtnRight.GetComponent<UIEventTrigger>().onDragOut.Add(new EventDelegate(player, "Stop"));

        BtnJump.GetComponent<UIEventTrigger>().onPress.Add(new EventDelegate(player, "PressJump"));
        BtnAttack.GetComponent<UIEventTrigger>().onPress.Add(new EventDelegate(player, "PressAttack"));
        BtnAttack.GetComponent<UIEventTrigger>().onRelease.Add(new EventDelegate(player, "ReleaseAttack"));
    }

    public void UpdateHp(float count)
    {
        float factor = count / maxHealth; 
        HealthBar.value = factor;
    }

    public void UpdateGold(int count)
    {
        GoldLabel.text = count.ToString();
    }

    public void UpdateSavedHostage(int count)
    {
        string dName = "HOSTAGE_" + count.ToString() + "_DISABLE";
        string eName = "HOSTAGE_" + count.ToString() + "_ENABLE";

        Transform hostage = Hostages.transform.FindChild(dName);
        hostage.gameObject.SetActive(false);

        hostage = Hostages.transform.FindChild(eName);
        hostage.gameObject.SetActive(true);

    }

    public void ResetSavedHostage()
    {
        for (int i = 0; i < 3; i++)
        {
            string dName = "HOSTAGE_" + i.ToString() + "_DISABLE";
            string eName = "HOSTAGE_" + i.ToString() + "_ENABLE";
            Hostages.transform.FindChild(dName).gameObject.SetActive(true);
            Hostages.transform.FindChild(eName).gameObject.SetActive(false);
        }
    }


	
}
