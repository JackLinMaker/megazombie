using UnityEngine;
using System.Collections;

public class Hostage : MonoBehaviour
{
    private SpriteRenderer renderer;
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
   

    public void Disappear()
    {
        BaseUI.Instance.AddHostage(1);
        StartCoroutine(disappearCo());
    }

  
    private IEnumerator disappearCo()
    {
        yield return new WaitForSeconds(1f);
        renderer.materials[0].SetColor("_Color", new Color(1, 1, 1, 1));
        yield return new WaitForSeconds(0.1f);
        renderer.materials[0].SetColor("_Color", new Color(1, 1, 1, 0.8f));
        yield return new WaitForSeconds(0.1f);
        renderer.materials[0].SetColor("_Color", new Color(1, 1, 1, 0.6f));
        yield return new WaitForSeconds(0.1f);
        renderer.materials[0].SetColor("_Color", new Color(1, 1, 1, 0.2f));
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

}
