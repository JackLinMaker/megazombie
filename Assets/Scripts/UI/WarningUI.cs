using UnityEngine;
using System.Collections;

public class WarningUI : MonoBehaviour {

    public UILabel Content;

    public void Show(string content)
    {
        Content.text = content;
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
