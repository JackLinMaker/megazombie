using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionUI : MonoBehaviour
{
    public GameObject MainLevelMenu;
    public GameObject SubLevelMenu;
    public GameObject MainLevelItem;
    public GameObject MainLevelGrid;
    public GameObject BtnStartGame;
    public GameObject WarningWindow;

    [HideInInspector]
    public SublevelItem SelectedLevel;

    //public GameObject SubLevelGrid;
    public GameObject Level1List;
    public GameObject Level2List;

    public BagUI Bag;

    private static SelectionUI _instance;

    //This is the public reference that other classes will use
    public static SelectionUI Instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SelectionUI>();
            return _instance;
        }
    }

    private Dictionary<int, GameObject> MainLevelItems = new Dictionary<int, GameObject>();

    void Start()
    {
        // 加载大关信息
        for (int i = 0; i < DataManager.Instance.MainLevel; i++)
        {
            GameObject item = Instantiate(MainLevelItem) as GameObject;
            item.GetComponent<MainLevelItem>().MainLevel = i;
            if (CheckMainLevelIsLock(item.GetComponent<MainLevelItem>().MainLevel) == true)
            {
                item.GetComponent<MainLevelItem>().SetLevelLock(true);
            }
            else
            {
                item.GetComponent<MainLevelItem>().SetLevelLock(false);
            }
            item.transform.parent = MainLevelGrid.transform;
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.localPosition = new Vector3(i * 550, 0, 0);
            EventDelegate ed = new EventDelegate(this, "UISubLevelShow");
            ed.parameters[0] = new EventDelegate.Parameter(i);
            ed.parameters[1] = new EventDelegate.Parameter(item);
            item.GetComponent<UIEventTrigger>().onRelease.Add(ed);
            MainLevelItems.Add(i, item);
        }

        SubLevelMenu.SetActive(true);
        Level1List.SetActive(true);
        SublevelItem[] levels1 = Level1List.GetComponentsInChildren<SublevelItem>();
        // 加载小关信息
        for (int i = 0; i < DataManager.Instance.SubLevel; i++)
        {
            levels1[i].LevelInfo = DataManager.Instance.GetLevelInfo(0, i);
            levels1[i].LevelName = "Level" + (0 + 1).ToString() + "-" + (i + 1).ToString();
            EventDelegate ed = new EventDelegate(this, "SelectLevel");
            ed.parameters[0] = new EventDelegate.Parameter(levels1[i]);
            levels1[i].gameObject.GetComponent<UIButton>().onClick.Add(ed);
            if (CheckSubLevelIsLock(levels1[i].LevelInfo) == true)
            {
                levels1[i].SetLevelLock(true);
            }
            else
            {
                levels1[i].SetLevelLock(false);
            }
            //levels1[i].SetLevelLock(true);
        }
        SubLevelMenu.SetActive(false);
        Level1List.SetActive(false);

        SubLevelMenu.SetActive(true);
        Level2List.SetActive(true);
        SublevelItem[] levels2 = Level2List.GetComponentsInChildren<SublevelItem>();
        // 加载小关信息
        for (int i = 0; i < DataManager.Instance.SubLevel; i++)
        {
            levels2[i].LevelInfo = DataManager.Instance.GetLevelInfo(1, i);
            levels2[i].LevelName = "Level" + (0 + 2).ToString() + "-" + (i + 1).ToString();
            EventDelegate ed = new EventDelegate(this, "SelectLevel");
            ed.parameters[0] = new EventDelegate.Parameter(levels2[i]);
            levels2[i].gameObject.GetComponent<UIButton>().onClick.Add(ed);
            if (CheckSubLevelIsLock(levels2[i].LevelInfo) == true)
            {
                levels2[i].SetLevelLock(true);
            }
            else
            {
                levels2[i].SetLevelLock(false);
            }
            //levels2[i].SetLevelLock(true);
        }
        SubLevelMenu.SetActive(false);
        Level2List.SetActive(false);

        //Bag.BasicCount.text = DataManager.Instance.PInfo.Basic.ToString();
        //Bag.AdvancedCount.text = DataManager.Instance.PInfo.Advanced.ToString();
        Bag.LoadWeapon();
    }

    public void LoadLevel()
    {
        if (SelectedLevel)
        {
            DataManager.Instance.CurrentMainLevel = SelectedLevel.LevelInfo.MainLevel + 1;
            DataManager.Instance.CurrentSubLevel = SelectedLevel.LevelInfo.SubLevel + 1;


            Application.LoadLevel(SelectedLevel.LevelName);


        }

    }

    public void SelectLevel(SublevelItem selectlevel)
    {
        if (SelectedLevel == selectlevel) return;
        SelectedLevel = selectlevel;
        if (selectlevel.IsLocked == true)
        {
            BtnStartGame.GetComponent<UIButton>().isEnabled = false;
        }
        else
        {
            BtnStartGame.GetComponent<UIButton>().isEnabled = true;
        }
        selectlevel.GetComponent<UIToggle>().Set(true);
    }

    public void UISubLevelShow(int mainLevel, GameObject button)
    {
        if (button.GetComponent<MainLevelItem>().isPress == false) return;
        if (CheckMainLevelIsLock(mainLevel))
        {
            WarningWindow.GetComponent<WarningUI>().Show("此关卡未解锁!");
            return;
        }

        MainLevelMenu.SetActive(false);
        SubLevelMenu.SetActive(true);
        button.GetComponent<MainLevelItem>().HidePress();
        BtnStartGame.SetActive(true);
        if (mainLevel == 0)
        {
            Level1List.SetActive(true);
            SublevelItem[] levels = Level1List.GetComponentsInChildren<SublevelItem>();
            SelectLevel(levels[0]);

        }
        else if (mainLevel == 1)
        {
            Level2List.SetActive(true);
            SublevelItem[] levels = Level2List.GetComponentsInChildren<SublevelItem>();
            SelectLevel(levels[0]);

        }

    }

    public void UISubLevelBack()
    {
        SubLevelMenu.SetActive(false);
        MainLevelMenu.SetActive(true);
        BtnStartGame.SetActive(false);
        Level1List.SetActive(false);
        Level2List.SetActive(false);
    }

    private bool CheckMainLevelIsLock(int mainLevel)
    {
        //if (mainLevel == 0 || mainLevel == 1)
        //{
        //    return false;
        //}


        if (DataManager.Instance.PInfo.MainLevel < mainLevel)
        {
            return true;
        }

        return false;
    }

    private bool CheckSubLevelIsLock(LevelInfo info)
    {
        if (DataManager.Instance.PInfo.MainLevel < info.MainLevel)
        {
            return true;
        }

        if (DataManager.Instance.PInfo.MainLevel == info.MainLevel)
        {
            if (DataManager.Instance.PInfo.SubLevel < info.SubLevel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

}
