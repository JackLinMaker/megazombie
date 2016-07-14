using UnityEngine;
using System.Collections;

public class InteractiveItem : MonoBehaviour
{

    public GameObject[] Contents;
    public GameObject[] Fragment;

    public float Health = 100f;

    public GameObject DestroyEffect;

    public enum InteractiveType
    {
        DESTROY_BOX,
        STATIC_BOX,
        PRISONCELL,
        BRICK,
    }

    public InteractiveType Type;

    public virtual void Hurt(float damage)
    {
        StartCoroutine(HurtColorChange());
        Health -= damage;

        if (Health <= 0)
        {
            if (DestroyEffect)
            {
                Instantiate(DestroyEffect, this.transform.position, Quaternion.identity);
            }
            addContents();
            addFragment();
            Destroy(gameObject);
        }


    }

    protected void addContents()
    {
        if (Contents.Length > 0)
        {
            for (int i = 0; i < Contents.Length; i++)
            {
                GameObject item = Instantiate(Contents[i], this.transform.position, Quaternion.identity) as GameObject;
                item.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(5f, 10f));
            }
        }
    }

    protected void addFragment()
    {
        for (int i = 0; i < Fragment.Length; i++)
        {
            GameObject item = Instantiate(Fragment[i], this.transform.position, Quaternion.identity) as GameObject;
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(8f, 10f));
            item.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(360.0f, 720.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.GetComponent<BaseBullet>();
            if (bullet.Owner == Util.ObjectOwner.PLAYER)
            {
                collider.GetComponent<BaseBullet>().DestoryWithEffect();
                if (Type != InteractiveType.STATIC_BOX)
                {
                    Hurt(collider.GetComponent<BaseBullet>().Damage);
                }

            }

        }
    }

    protected IEnumerator HurtColorChange()
    {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.5f);
        this.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.red);
        yield return new WaitForSeconds(0.08f);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.3f);
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.15f);
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
    }
}
