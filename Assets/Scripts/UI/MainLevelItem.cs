using UnityEngine;
using System.Collections;

public class MainLevelItem : MonoBehaviour
{

    public int MainLevel;
    public bool IsLock { get; private set; }
    public GameObject PressEffect;
    public GameObject Lock;
    public bool isPress = false;

    public void ShowPress()
    {
        //if (IsLock) return;
        PressEffect.SetActive(true);
        isPress = true;

    }

    public void HidePress()
    {
        //if (IsLock) return;
        PressEffect.SetActive(false);
        isPress = false;
    }

    public void SetLevelLock(bool islock)
    {
        if (islock)
        {
            IsLock = true;
            if (MainLevel == 2 || MainLevel == 3 || MainLevel == 4)
            {
                this.transform.GetComponent<UISprite>().spriteName = "imgSelectLevelBackground";
            }
            else
            {
                this.transform.GetComponent<UISprite>().spriteName = "img_mainLevel" + (MainLevel + 1).ToString();
            }


            Lock.SetActive(true);
        }
        else
        {
            IsLock = false;
            this.transform.GetComponent<UISprite>().spriteName = "img_mainLevel" + (MainLevel + 1).ToString();
            Lock.SetActive(false);
        }
    }

    void Start()
    {

    }

    void Update()
    {

        if (isPress)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetAxis("Mouse X") != 0)
            {
                HidePress();
            }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                HidePress();
            }
#endif
        }
    }


}
