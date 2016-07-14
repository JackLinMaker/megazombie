using UnityEngine;
using System.Collections;

public class DestroyerAnimationController : MonoBehaviour {

    private Destroyer destroyer;

    void Awake()
    {
        destroyer = this.transform.parent.GetComponent<Destroyer>();
    }

    public void Dead()
    {
        destroyer.Dead();
    }
}
