using UnityEngine;
using System.Collections;

public class Poisongas : MonoBehaviour
{
    public float DamageInterval;
    public float Damage;
    public GameObject Smoke;
    private float elapsedInterval;

    void Start()
    {
        StartCoroutine(Diffuse());
    }

    private IEnumerator Diffuse()
    {
        yield return new WaitForSeconds(0.3f);
        Smoke.transform.localScale = new Vector3(2, 1, 1);
        this.GetComponent<BoxCollider2D>().size = new Vector2(2, 2);
        yield return new WaitForSeconds(0.5f);
        Smoke.transform.localScale = new Vector3(3, 1, 1);
        this.GetComponent<BoxCollider2D>().size = new Vector2(3, 2);
        yield return new WaitForSeconds(0.7f);
        Smoke.transform.localScale = new Vector3(4, 1, 1);
        this.GetComponent<BoxCollider2D>().size = new Vector2(4, 2);
        yield return new WaitForSeconds(1f);
        Smoke.transform.localScale = new Vector3(5, 1, 1);
        this.GetComponent<BoxCollider2D>().size = new Vector2(5, 2);

        yield return new WaitForSeconds(3f);
        Smoke.GetComponent<ParticleSystem>().maxParticles = 70;
        yield return new WaitForSeconds(0.2f);
        Smoke.GetComponent<ParticleSystem>().maxParticles = 50;
        yield return new WaitForSeconds(0.2f);
        Smoke.GetComponent<ParticleSystem>().maxParticles = 30;
        yield return new WaitForSeconds(0.2f);
        Smoke.GetComponent<ParticleSystem>().maxParticles = 10;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
    void OnTriggerStay2D(Collider2D collider)
    {

        string tag = collider.gameObject.tag;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            elapsedInterval -= Time.deltaTime;
            if (elapsedInterval <= 0)
            {
                elapsedInterval = DamageInterval;
                Debug.Log("Hurt");
                Vector3 pos = new Vector3(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y + collider.bounds.size.y * 0.5f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.GetComponent<BaseEntity>().Hurt(Damage, pos, 1);
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            elapsedInterval -= Time.deltaTime;
            if (elapsedInterval <= 0)
            {
                elapsedInterval = DamageInterval;

                collider.gameObject.transform.GetComponent<Player>().Hurt(Damage);
            }
        }

    }
}
