using UnityEngine;
using System.Collections;

public class DropBlood : MonoBehaviour
{
    public Transform Position;
    //public GameObject DestroyEffect;
    public Collider2D Blood;
    public bool IsHaveBlood = true;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(360.0f, 720.0f);
        if (IsHaveBlood)
        {
            StartCoroutine(Drop());
        }
        
        Disapper();
    }

    void Update()
    {

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

    private IEnumerator Drop()
    {
        int count = Random.Range(2, 4);
        Collider2D[] cols = new Collider2D[count];
        for (int i = 0; i < count; i++)
        {
            Collider2D col = Instantiate(Blood, Position.position, Quaternion.identity) as Collider2D;
            col.gameObject.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity * Random.Range(1f, 1.5f);
            yield return null;
        }
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.GetComponent<Saw>())
    //    {
    //        if (DestroyEffect)
    //        {
    //            GameObject Effect = Instantiate(DestroyEffect, transform.position, Quaternion.identity) as GameObject;
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}
