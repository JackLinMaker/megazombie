using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUI : MonoBehaviour
{
    public GameObject BtnLeft;
    public GameObject BtnRight;
    public GameObject BtnJump;
    public GameObject BtnAttack;

    public GameObject GameMenu;
    public GameObject StatisticsMenu;
    public GameObject DeadMenu;
    public UILabel MaxBulletCount;
    public UILabel CurrentBulletCount;
    public UISprite Infinite;
    public GameObject Spr;
    //public UISprite CurrentWeaponPicture;
    //public GameObject PlayerHealthBar;
    //public GameObject BossHealthBar;
    //public GameObject BossInfo;
    public UILabel CollectGoldCount;
    public UILabel CollectDiamandCount;
    public UILabel TimeCount;
    public Transform Stamp;
    public GameObject BtnStaQuit;
    public GameObject BtnStaContinu;
    public GameObject StaGrid;
    public GameObject HostageGrid;
    public Transform EnemyItem;
    public Transform HostageItem;
    public Transform HostageItemInBaseUI;
    public GameObject Key;
    public Transform Zombie;
    public Transform Soldier;
    public Transform SoundSeeker;
    public Transform Bomber;
    public Transform Shielder;
    public Transform Crasher;
    public Transform Boss1;

    private List<string> killedEnemy;
    private float Seconds;
    private bool isTiming = false;
    private int savedHostage;
    private float collectGold;
  
    //private UISprite[] playerHealths;
    //private UISprite[] bossHealths;
    private Player player;
    private bool isContinue = false;
    private static BaseUI _instance;

    //This is the public reference that other classes will use
    public static BaseUI Instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<BaseUI>();
            return _instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
       
        player = GameObject.FindObjectOfType<Player>();

        // 虚拟轴挂接事件
        AddPlayerControl();

        Seconds = 0;
        isTiming = true;
      
        savedHostage = 0;
        killedEnemy = new List<string>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            Seconds += Time.deltaTime;
        }
    }

    public void AddPlayerControl()
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

    public void UpdateUICurrentWeaponPicture(string Name)
    {
        //CurrentWeaponPicture.spriteName = Name;
    }

    public void OpenGameMenu()
    {
        Time.timeScale = 0;
        GameMenu.SetActive(true);
    }

    public void CloseGameMenu()
    {
        Time.timeScale = 1;
        GameMenu.SetActive(false);
    }

    public void OpenStatisticsMenu()
    {
        isTiming = false;
        StatisticsMenu.SetActive(true);
        StartCoroutine(ShowKilledEnemyCo());
       
        DataManager.Instance.PInfo.Gold += (int)collectGold;

        //DataManager.Instance.PInfo.MainLevel = int.Parse(DataManager.Instance.GetNextLevelName().Substring(5, 1)) - 1;
        //DataManager.Instance.PInfo.SubLevel = int.Parse(DataManager.Instance.GetNextLevelName().Substring(7, 1)) - 1;
        DataManager.Instance.SavePlayerInfo();
        CollectGoldCount.text = "0";
        CollectDiamandCount.text = "0";
    }


    private float gold = 0;
    private float diamand = 0;
    private Transform GetStaItem(string tag)
    {
        if (tag == "Zombie")
        {
            return Instantiate(Zombie) as Transform;
        }
        else if (tag == "Soldier")
        {
            return Instantiate(Soldier) as Transform;
        }
        else if (tag == "SoundSeeker")
        {
            return Instantiate(SoundSeeker) as Transform;
        }
        else if (tag == "Bomber")
        {
            return Instantiate(Bomber) as Transform;
        }
        else if (tag == "Shielder")
        {
            return Instantiate(Shielder) as Transform;
        }
        else if (tag == "Crasher")
        {
            return Instantiate(Crasher) as Transform;
        }
        else if (tag == "Boss-1")
        {
            return Instantiate(Boss1) as Transform;
        }
        else if (tag == "Destroyer")
        {
            return Instantiate(Boss1) as Transform;
        }
        else if (tag == "HeavyMachiner")
        {
            return Instantiate(Boss1) as Transform;
        }
        return null;
    }

    private Vector3 vec = new Vector3(1, 1, 1);

    private IEnumerator ShowKilledEnemyCo()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < killedEnemy.Count; i++)
        {
            Transform item = GetStaItem(killedEnemy[i]);
            item.GetComponent<CharacterController2D>().enabled = false;
            item.GetComponent<BaseEntity>().enabled = false;
            item.parent = StaGrid.transform;
            item.localScale = new Vector3(50, 50, 1);
            if (i < 20)
            {
                item.localPosition = new Vector3(-185 + i * 22, 42, 0);
            }
            else
            {
                item.localPosition = new Vector3(-185 + (i - 20) * 22, -42, 0);
            }
            Director.Instance.ShakeScreen();
            item.FindChild("Animator").GetComponent<Animator>().SetBool("Dead", true);
            item.GetComponent<BaseEntity>().addDeadEffectDebug();
            yield return new WaitForSeconds(0.15f);
        }

       
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < savedHostage; i++)
        {
            Transform item = Instantiate(HostageItem) as Transform;
            item.parent = HostageGrid.transform;
            item.localScale = new Vector3(1, 1, 1);
            item.localPosition = new Vector3(-185 + i * 30, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        TimeCount.text = ((int)(Seconds / 60)).ToString() + "分" + ((int)(Seconds % 60)).ToString() + "秒";
        TimeCount.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        Stamp.gameObject.SetActive(true);
        while (Stamp.localScale.x > 1)
        {
            Stamp.localScale = Vector3.Lerp(Stamp.localScale, vec, 0.5f);
            if (Stamp.localScale.x - 1 < 0.1)
            {
                Stamp.localScale = new Vector3(1, 1, 1);
            }
            yield return null;
        }
        Director.Instance.ShakeScreen();
        yield return new WaitForSeconds(0.3f);
        BtnStaQuit.SetActive(true);
        BtnStaContinu.SetActive(true);
    }

  
    public void ContinueGame()
    {
        if (isContinue == true) 
            return;
        isContinue = true;
        string next = DataManager.Instance.GetNextLevelName();
        PlayerPrefs.SetString("NextScene", next);
        Director.Instance.GoToNextSence("SplashScene");
        DataManager.Instance.CurrentMainLevel = int.Parse(next.Substring(5, 1));
        DataManager.Instance.CurrentSubLevel = int.Parse(next.Substring(7, 1));
    }

    public void Replay()
    {
        Time.timeScale = 1;
        DataManager.Instance.PInfo.Gold -= (int)collectGold;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.LoadLevel("LevelSelectionScene");
    }

    // num:要获取的数字，n：个位1，十位2，以此类推
    private int FindNum(int num, int n)
    {
        int power = (int)Mathf.Pow(10, n);
        return (num - num / power * power) * 10 / power;
    }

    public void AddHostage(int count)
    {
        savedHostage += count;
        Transform item = Instantiate(HostageItemInBaseUI) as Transform;
        item.parent = this.transform;
        item.localPosition = new Vector3(-400 + savedHostage * 40, 140, 0);
    }

    public void AddKey()
    {
        Key.SetActive(true);
    }

    public void HideKey()
    {
        Key.SetActive(false);
    }

    public void AddKilledEnemy(string tag)
    {
        killedEnemy.Add(tag);
    }

}
