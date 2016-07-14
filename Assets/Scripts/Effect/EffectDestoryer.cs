using UnityEngine;
using System.Collections;

public class EffectDestoryer : MonoBehaviour {

    public float DestoryTime;

	// Use this for initialization
	void Start () 
    {
        Destroy(this.gameObject, DestoryTime);
	}

}
