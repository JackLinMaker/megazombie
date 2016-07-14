using UnityEngine;
using System.Collections;

public class ShellCase : MonoBehaviour 
{
    public float TotalTime;

    private float elapsedTime;
    private TweenAlpha tween;
    
    void Awake()
    {
        tween = GetComponent<TweenAlpha>();
        StartCoroutine(Eliminate());
    }

    /*void Update()
    {
        TotalTime -= Time.deltaTime;
        Debug.Log("TotalTime = " + TotalTime);
        if (TotalTime <= 0.0f)
        {
            
            Destroy(gameObject);
        }
    }*/


    IEnumerator Eliminate()
    {
        yield return new WaitForSeconds(3.0f);
        tween.enabled = true;
        tween.PlayForward();
    }

    public void OnFinished()
    {
        Destroy(gameObject);
       
    }
	
}
