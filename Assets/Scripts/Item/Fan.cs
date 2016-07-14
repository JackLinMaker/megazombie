using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour
{
    public float Force;
    public float EnterForce;
    public enum FanType
    {
        UP,
        Down,
        Left,
        Right
    }
    public FanType Type;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Type == FanType.UP)
            {
                collider.GetComponent<Player>().Controller.AddForce(Vector2.up * Force);
            }
            else if (Type == FanType.Down)
            {
                collider.GetComponent<Player>().Controller.AddForce(-Vector2.up * Force);
            }
            else if (Type == FanType.Left)
            {
                collider.GetComponent<Player>().Controller.SetDragForce(-Vector2.right * Force * 4);
            }
            else if (Type == FanType.Right)
            {
                collider.GetComponent<Player>().Controller.SetDragForce(Vector2.right * Force * 4);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (Type == FanType.UP)
            {
                collider.GetComponent<Player>().Controller.SetForce(Vector2.up * EnterForce);
            }
            else if (Type == FanType.Down)
            {
                collider.GetComponent<Player>().Controller.SetForce(-Vector2.up * EnterForce);
            }
            else if (Type == FanType.Left)
            {

            }
            else if (Type == FanType.Right)
            {

            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (Type == FanType.UP)
            {

            }
            else if (Type == FanType.Down)
            {

            }
            else if (Type == FanType.Left)
            {
                collider.GetComponent<Player>().Controller.SetDragForce(Vector2.zero);
            }
            else if (Type == FanType.Right)
            {
                collider.GetComponent<Player>().Controller.SetDragForce(Vector2.zero);
            }
        }
    }
}
