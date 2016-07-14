using UnityEngine;
using System.Collections;

public class HiddenArea : MonoBehaviour 
{
    private MeshRenderer renderer;
    //private bool isFading = false;

    void Awake()
    {

        renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //isFading = true;
            StopAllCoroutines();
            StartCoroutine("FadeOut");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StopAllCoroutines();
            StartCoroutine("FadeIn");
        }
    }

    IEnumerator FadeOut()
    {
        Color color = renderer.material.color;
        while (color.a > 0.3f)
        {
            yield return new WaitForSeconds(0.02f);
            color.a -= 0.1f;
            renderer.material.color = color;
        }
        //isFading = false;
        yield return null;
    }

    IEnumerator FadeIn()
    {
        Color color = renderer.material.color;
        while (color.a < 1.0f)
        {
            yield return new WaitForSeconds(0.02f);
            color.a += 0.1f;
            renderer.material.color = color;
        }
        //isFading = false;
        yield return null;
    }


	
}
