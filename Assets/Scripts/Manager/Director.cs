using UnityEngine;
using System.Collections;


public class Director : MonoBehaviour
{
    public static Director Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(Director)) as Director;
                if (instance == null)
                    Debug.Log("Could not locate an Director object. \n You have to have exactly one Director in the scene.");
            }
            return instance;
        }
    }

    public Player Player;
    public Transform Camera;
    public TweenAlpha SenceMask;
    public CheckPoint DebugSpawn;

    // UI
    public HUDPanel HUD;
    public GameObject PauseMenu;
    public GameObject ReviveMenu;

    private static Director instance = null;
    private EventDelegate gotoNextSenceCallback;
    private EventDelegate gotoNextMapCallback;
    private string nextSenceName;

    public Transform[] CheckPoints;
    private int currentCheckPointIndex;
    

    void Start()
    {
        SenceMask.gameObject.SetActive(true);
        SenceMask.PlayForward();

        // add check points
        currentCheckPointIndex = -1;
    }

    public void HitCheckPoint()
    {
        currentCheckPointIndex += 1;
        Debug.Log("currentCheckPointIndex = " + currentCheckPointIndex);
    }

    public Transform GetLastCheckPoint()
    {
        if (currentCheckPointIndex < CheckPoints.Length)
        {
            Debug.Log("GetLastCheckPoint = " + currentCheckPointIndex);
            return CheckPoints[currentCheckPointIndex];
        }

        return null;
    }

  
    void OnApplicationQuit()
    {
        instance = null;
    }

    

    public void GiveControl()
    {
        Player.IsControl = true;
        Camera.GetComponent<CameraController>().IsControl = true;
    }

    public void TakeControl()
    {
        Player.IsControl = false;
        Player.StopMoving();
        Camera.GetComponent<CameraController>().IsControl = false;
    }

    public void GoToNextSence(string name)
    {
        nextSenceName = name;

        SenceMask.PlayReverse();
        gotoNextSenceCallback = new EventDelegate(this, "MoveNextSence");
        SenceMask.AddOnFinished(gotoNextSenceCallback);
    }

    private void MoveNextSence()
    {
        Application.LoadLevel(nextSenceName);
        SenceMask.RemoveOnFinished(gotoNextSenceCallback);
    }

  

    public void ShakeScreen()
    {
        Camera.GetComponent<CameraCanShake>().originalCameraPosition = Camera.position;
        Camera.transform.GetComponent<CameraCanShake>().SimpleShake();
    }

    public void JumpShake()
    {
        Camera.GetComponent<CameraCanShake>().originalCameraPosition = Camera.position;
        Camera.transform.GetComponent<CameraCanShake>().JumpShake();
    }

    public void ShowPause()
    {
        Debug.Log("Show Pause");
        PauseMenu.gameObject.SetActive(true);
    }

    public void HidePause()
    {
        Debug.Log("Hide Pause");
        PauseMenu.gameObject.SetActive(false);
    }

    public void ShowRevive()
    {
        Debug.Log("Show Revive");
        ReviveMenu.gameObject.SetActive(true);
    }
    
    public void HideRevive()
    {
        Debug.Log("Hide Revive");
        ReviveMenu.gameObject.SetActive(false);
    }

    public void Revive()
    {
        Debug.Log("Revive");
        Player.Revive();
        HideRevive();
    }

    public void Resume()
    {
        Debug.Log("Resume");
        Director.Instance.HidePause();
    }

    public void Restart()
    {
        Debug.Log("Restart");
        Time.timeScale = 1;

        Application.LoadLevel(Application.loadedLevel);

    }

    public void Quit()
    {
        Debug.Log("Quit");
        Time.timeScale = 1;
        //Application.LoadLevel("LevelSelectionScene");
    }

    
}
