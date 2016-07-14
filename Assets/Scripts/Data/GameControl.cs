using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class PlayerData
{

    public int experiencePoints;
    public int playerLevel;
}

public class GameControl : MonoBehaviour 
{
    public static GameControl control;
    
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

   

    /*public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat",
                                    FileMode.Open);

        PlayerData data = new PlayerData();
        data.experiencePoints = experiencePoints;
        data.playerLevel = playerLevel;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat",
                                        FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            experiencePoints = data.experiencePoints;
            playerLevel = data.playerLevel;
        }
    }*/
    
	
}

