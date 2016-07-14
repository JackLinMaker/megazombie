using UnityEngine;
using System.Collections;

public class EndMark : MonoBehaviour 
{
    public GameObject Flag;
    public GameObject Pole;

    public Transform Destination;

    public void Win()
    {
        Flag.transform.GetComponent<BoxCollider2D>().enabled = false;
        Pole.transform.GetComponent<BoxCollider2D>().enabled = false;

        Flag.transform.GetComponent<FollowPath>().enabled = false;
        Director.Instance.Player.IsControl = false;
        Director.Instance.Player.MoveToRight();
    }

    void Update()
    {
        if (Mathf.Abs(Director.Instance.Player.transform.position.x - Destination.position.x) < 0.1f)
        {
            Director.Instance.Player.StopMoving();
        }
    }
}
