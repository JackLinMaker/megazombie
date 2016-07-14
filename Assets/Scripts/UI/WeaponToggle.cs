using UnityEngine;
using System.Collections;

public class WeaponToggle : MonoBehaviour {

    public string initName;
    public string checkedName;

    public void IsActive()
    {
        if (UIToggle.current.value)
        {
            this.GetComponent<UISprite>().spriteName = checkedName;
        }
        else
        {
            this.GetComponent<UISprite>().spriteName = initName;
        }

    }
}
