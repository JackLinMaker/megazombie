using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

public class GlobalDataHelper
{
    private const string DATA_ENCRYPT_KEY = "a234857890654c3678d77234567890O2";
    private static RijndaelManaged _encryptAlgorithm = null;

    public static RijndaelManaged DataEncryptAlgorithm()
    {
        _encryptAlgorithm = new RijndaelManaged();
        _encryptAlgorithm.Key = Encoding.UTF8.GetBytes(DATA_ENCRYPT_KEY);
        _encryptAlgorithm.Mode = CipherMode.ECB;
        _encryptAlgorithm.Padding = PaddingMode.PKCS7;

        return _encryptAlgorithm;
    }
}

public class DataManager : MonoBehaviour
{
    // Standard Singleton
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<DataManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public bool IsDebug = false;

    void Awake()
    {
        playerPath = Application.persistentDataPath + "/playerInfo.dat";
        levelPath = Application.persistentDataPath + "/levelInfo.dat";
        weaponPath = Application.dataPath + "/Resources/Weapon/weaponInfo.txt";

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        LoadData();
    }

    void Start()
    {

    }

    public PlayerInfo PInfo;
    //public List<LevelInfo> Levels = new List<LevelInfo>();
    public Dictionary<string, LevelInfo> Levels = new Dictionary<string, LevelInfo>();
    public Dictionary<int, WeaponInfo> Weapons = new Dictionary<int, WeaponInfo>();
    public List<BaseEntity.EnemyType> KilledEnemyList = new List<BaseEntity.EnemyType>();
    public Dictionary<int, Color> LevelBackgroundColors = new Dictionary<int, Color>();
    public int MainLevel = 5;
    public int SubLevel = 5;
    public bool isEncrypt = false;
    public int CurrentMainLevel = 0;
    public int CurrentSubLevel = 0;

    private string playerPath;
    private string levelPath;
    private string weaponPath;


    private void WriteFile(string path, string str)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            if (isEncrypt)
            {
                writer.Write(EncryptData(str));
            }
            else
            {
                writer.Write(str);
            }

        }
    }

    private string ReadFile(string path)
    {

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                if (isEncrypt)
                {
                    return DecryptData(reader.ReadToEnd());
                }
                else
                {
                    return reader.ReadToEnd();
                }

            }
        }
        else
        {
            Debug.Log("File is not exists");
            return string.Empty;
        }
    }


    public void LoadData()
    {
        LoadLevelInfos();
        //LoadWeaponInfos();
        LoadPlayerInfo();
        //LoadEntityInfos();

    }

    public void LoadEntityInfos()
    {
        TextAsset ob = (TextAsset)Resources.Load("Entity/EntityInfo");
        string str = ob.text;

        EntityInfo[] infos = JsonMapper.ToObject<EntityInfo[]>(str);
        for (int i = 0; i < infos.Length; i++)
        {

            string path = "Entity/" + infos[i].Name;
            Debug.Log("path = " + path);
            BaseEntity entity = Resources.Load<BaseEntity>(path);
            entity.Damage = (float)infos[i].Damage;
            entity.CurrentHealth = (float)infos[i].CurrentHealth;
           
            entity.SpeedAccelerationOnGround = (float)infos[i].SpeedOnGround;
            entity.SpeedAccelerationInAir = (float)infos[i].SpeedOnAir;
            entity.ChaseRange = (float)infos[i].ChaseRange;
            entity.ChaseFactor = (float)infos[i].ChaseFactor;
            entity.AttackRange = (float)infos[i].AttackRange;
            entity.PauseTime = (float)infos[i].PauseTime;
            
            Perspective perspective = entity.GetComponent<Perspective>();
            perspective.faceRayDistance = (float)infos[i].FaceRayDistance;
            perspective.backRayDistance = (float)infos[i].BackRayDistance;
            perspective.detectionRate = (float)infos[i].DetectionRate;
        }

    }

    public void InitializePlayerInfo()
    {
        Debug.Log("InitializePlayerInfo");
        PInfo = new PlayerInfo();
       
        PInfo.MainLevel = 1;
        PInfo.SubLevel = 4;
        PInfo.BuyedWeapons = new List<int>();
        PInfo.BuyedWeapons.Add(7);
        PInfo.BuyedWeapons.Add(1);
        PInfo.MWeapon = Weapons[7];
        PInfo.RWeapon = Weapons[1];

        string str = JsonMapper.ToJson(PInfo);
        Debug.Log("str = " + str);
        WriteFile(playerPath, str);
    }

    public void LoadPlayerInfo()
    {
        if (File.Exists(playerPath))
        {
            string str = ReadFile(playerPath);
            Debug.Log("LoadPlayerInfo str = " + str);
            PInfo = JsonMapper.ToObject<PlayerInfo>(str);
        
        }
        else
        {
            InitializePlayerInfo();
        }
    }

    public void SavePlayerInfo()
    {
        string str = JsonMapper.ToJson(PInfo);
        Debug.Log("SavePlayerInfo str = " + str);
        WriteFile(playerPath, str);
    }

    public LevelInfo GetLevelInfo(int mainLevel, int subLevel)
    {
        string key = mainLevel.ToString() + "-" + subLevel.ToString();
        return Levels[key];
    }

    public void SaveLevelInfos()
    {
        JsonData datas = new JsonData();
        foreach (KeyValuePair<string, LevelInfo> level in Levels)
        {
            datas[level.Key] = JsonMapper.ToJson(level.Value);
        }

        WriteFile(levelPath, JsonMapper.ToJson(datas));
    }

    public void LoadLevelInfos()
    {

        if (File.Exists(levelPath))
        {
            string str = ReadFile(levelPath);
            Debug.Log("LoadLevelInfo str = " + str);
            JsonData datas = JsonMapper.ToObject(str);
            for (int i = 0; i < MainLevel; i++)
            {
                for (int j = 0; j < SubLevel; j++)
                {
                    string key = i.ToString() + "-" + j.ToString();
                    Debug.Log("key = " + key);

                    string data = (string)datas[key];
                    Debug.Log("data = " + data);

                    LevelInfo level = JsonMapper.ToObject<LevelInfo>(data);
                    Levels.Add(key, level);

                }
            }
        }
        else
        {
            InitializeLevelInfos();
        }

        LevelBackgroundColors.Add(1, new Color((float)165 / 255, (float)210 / 255, (float)236 / 255, (float)5 / 255));
        LevelBackgroundColors.Add(2, new Color((float)12 / 255, (float)25 / 255, (float)45 / 255, (float)18 / 255));
    }

    public void LoadWeaponInfos()
    {

        TextAsset ob = (TextAsset)Resources.Load("Weapon/WeaponInfo");
        string str = ob.text;

        WeaponInfo[] infos = JsonMapper.ToObject<WeaponInfo[]>(str);
        for (int i = 0; i < infos.Length; i++)
        {
            Weapons.Add((int)infos[i].Id, infos[i]);
            string path = "Weapon/" + infos[i].Name;
            BaseGun baseGun = Resources.Load<BaseGun>(path);
            baseGun.Rate = (float)infos[i].Rate;
            baseGun.Speed = (float)infos[i].BulletSpeed;
            baseGun.Damage = (float)infos[i].Damage;
            baseGun.Range = (float)infos[i].Range;
            baseGun.HitForceX = (float)infos[i].HitForceX;
            baseGun.HitForceY = (float)infos[i].HitForceY;
            baseGun.IsAutomatic = infos[i].IsAutomatic;
        }
    }

    public void SaveWeaponInfos()
    {
        WeaponInfo[] infos = new WeaponInfo[DataManager.Instance.Weapons.Count];

        foreach (KeyValuePair<int, WeaponInfo> weapon in DataManager.Instance.Weapons)
        {
            infos[weapon.Key - 1] = weapon.Value;
        }
        string json = JsonMapper.ToJson(infos);
        WriteFile(weaponPath, json);
        //AssetDatabase.Refresh();
    }

    public void InitializeLevelInfos()
    {
        JsonData datas = new JsonData();
        for (int i = 0; i < MainLevel; i++)
        {
            for (int j = 0; j < SubLevel; j++)
            {
                string key = i.ToString() + "-" + j.ToString();
                LevelInfo level = new LevelInfo();
                level.MainLevel = i;
                level.SubLevel = j;
                level.Name = "黄昏";
                level.IsComplete = false;
                level.Result = LevelInfo.ResultType.None;
                Levels.Add(key, level);

                string str = JsonMapper.ToJson(level);
                datas[key] = str;

            }
        }

        WriteFile(levelPath, JsonMapper.ToJson(datas));

    }

    public string EncryptData(string dataToEncrypt)
    {
        byte[] dataToEncryptArray = Encoding.UTF8.GetBytes(dataToEncrypt);
        byte[] dataAfterEncryptArray = GlobalDataHelper.DataEncryptAlgorithm().CreateEncryptor().TransformFinalBlock(dataToEncryptArray, 0, dataToEncryptArray.Length);
        return Convert.ToBase64String(dataAfterEncryptArray, 0, dataAfterEncryptArray.Length);
    }

    public string DecryptData(string dataToDecrypt)
    {
        byte[] dataToDecryptArray = Convert.FromBase64String(dataToDecrypt);
        byte[] dataAfterDecryptArray = GlobalDataHelper.DataEncryptAlgorithm().CreateDecryptor()
    .TransformFinalBlock(dataToDecryptArray, 0, dataToDecryptArray.Length);
        return Encoding.UTF8.GetString(dataAfterDecryptArray);
    }

    public string GetNextLevelName()
    {
        int nextSubLevel = 0;
        int nextMainLevel = 0;

        if (CurrentMainLevel == MainLevel && CurrentSubLevel == SubLevel)
        {
            return null;
        }

        if (CurrentSubLevel == SubLevel)
        {
            nextSubLevel = 1;
            nextMainLevel = CurrentMainLevel + 1;
        }
        else
        {
            nextSubLevel = CurrentSubLevel + 1;
            nextMainLevel = CurrentMainLevel;
        }
        return "Level" + nextMainLevel.ToString() + "-" + nextSubLevel.ToString();
    }

}
