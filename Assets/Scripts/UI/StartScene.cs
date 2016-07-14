using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {

    public void TapToStart()
    {
        //GameDataManager.Instance.LoadData();
        Application.LoadLevel("LevelSelectionScene");
    }
}
