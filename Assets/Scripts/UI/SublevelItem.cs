using UnityEngine;
using System.Collections;

public class SublevelItem : MonoBehaviour
{

    public LevelInfo LevelInfo;
    public string LevelName;
    public bool IsLocked { get; private set; }

    public void SetLevelLock(bool islock)
    {
        if (islock)
        {
            IsLocked = true;
            this.transform.GetComponent<UISprite>().spriteName = "btn_" + LevelName + "disable";
        }
        else
        {
            IsLocked = false;
            this.transform.GetComponent<UISprite>().spriteName = "btn_" + LevelName;
        }
    }
}
