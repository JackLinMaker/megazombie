using UnityEngine;
using System.Collections;

public class GameMainUI : MonoBehaviour
{

    public GameObject SelectionUI;
    public GameObject BagUI;

    public void UISelectionToUIBag()
    {
        BagUI.SetActive(true);
        SelectionUI.SetActive(false);
    }

    public void UIBagToUISelection()
    {
        DataManager.Instance.SavePlayerInfo();
        SelectionUI.SetActive(true);
        BagUI.SetActive(false);
    }
}
