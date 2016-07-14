using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour {
    public float Speed;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, -1f, 0) * Time.deltaTime * Speed);
	}

    public void CloseRain()
    {
        StartCoroutine(Close());
    }

    public IEnumerator Close()
    {
        yield return new WaitForSeconds(1.5f);
        transform.gameObject.SetActive(false);
    }
}
