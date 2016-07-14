using UnityEngine;
using System.Collections;

public class BloodStain : MonoBehaviour
{

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
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

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "HCollision")
        {
            rigidbody2D.isKinematic = true;
        }
        else if (collider.gameObject.tag == "VCollision")
        {
            rigidbody2D.isKinematic = true;
            transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        }
    }
}
