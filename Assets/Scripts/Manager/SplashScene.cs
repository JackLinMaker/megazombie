using UnityEngine;
using System.Collections;

public class SplashScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        string level = PlayerPrefs.GetString("NextScene");
        Debug.Log(level.ToString());
        Application.LoadLevel(level);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
