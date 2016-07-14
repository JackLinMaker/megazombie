using UnityEngine;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    public string NextScene;
    public bool IsSaveData = false;
    public Transform Destination;

    private Transform player;

    void Awake()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            Director.Instance.Player.IsControl = false;
            Director.Instance.Player.MoveToRight();
        }
    }

    void Update()
    {
        if (player)
        {
            if (Mathf.Abs(player.position.x - Destination.position.x) < 0.1)
            {
                Director.Instance.Player.StopMoving();
                //StartCoroutine(openStaMenuCo());
                //player = null;
            }
        }
    }
    private IEnumerator openStaMenuCo()
    {
        yield return new WaitForSeconds(0.5f);
        BaseUI.Instance.OpenStatisticsMenu();
    }
}
