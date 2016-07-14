using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject UI;
    public GameObject Character;

    public List<BaseEntity> Enemys { get; set; }
    //Here is a private reference only this class can access
    private static GameManager _instance;

    //This is the public reference that other classes will use
    public static GameManager instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    void Awake()
    {
        Enemys = new List<BaseEntity>();
    }

    void Start()
    {
        BaseEntity[] enemys = GameObject.FindObjectsOfType<BaseEntity>();
        for (int i = 0; i < enemys.Length; i++)
        {
            Enemys.Add(enemys[i]);
        }
    }

    public void ReloadLevel()
    {
        StartCoroutine(showGameMenu());
    }

    IEnumerator showGameMenu()
    {
        yield return new WaitForSeconds(1.0f);
        BaseUI.Instance.OpenGameMenu();
        
    }

    public void TimeSlowDown()
    {
        StartCoroutine(timeSlowDown());
    }

    IEnumerator timeSlowDown()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = 1f;
    }
}

