using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Disapper();
	}

    public void Disapper()
    {
        StartCoroutine(DisapperCo());
    }

    private IEnumerator DisapperCo()
    {
        yield return new WaitForSeconds(5f);

        this.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1, 0.8f));

        yield return new WaitForSeconds(0.08f);

        this.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1, 0.6f));

        yield return new WaitForSeconds(0.08f);

        this.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1, 0.4f));

        yield return new WaitForSeconds(0.08f);

        this.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1, 0.2f));

        yield return new WaitForSeconds(0.08f);
        Destroy(this.gameObject);
    }
}
