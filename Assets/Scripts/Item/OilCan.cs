using UnityEngine;
using System.Collections;

public class OilCan : InteractiveItem
{

    public enum ExplosionType
    {
        Immediate,
        Delay,
    }

    public ExplosionType ExType;

    public float Damage;

    public Vector2 HitForce;

    public float Radius;

    public GameObject OnFireEffect;

    public LayerMask Mask;

    public float delayTime;

    public override void Hurt(float damage)
    {

        if (ExType == ExplosionType.Immediate)
        {

            Destroy(gameObject);
            explosion();
            addContents();

        }
        else if (ExType == ExplosionType.Delay)
        {

            StartCoroutine(delay());

        }
    }

    IEnumerator delay()
    {
        GameObject fire = Instantiate(OnFireEffect, transform.position + new Vector3(0, -0.5f, 0), Quaternion.Euler(-90.0f, 0.0f, 0.0f)) as GameObject;
        fire.transform.parent = this.transform;
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
        explosion();
        addContents();


    }

    private void explosion()
    {
        Director.Instance.ShakeScreen();
        Instantiate(DestroyEffect, this.transform.position - new Vector3(0, 0.66f, 0), Quaternion.identity);
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, Radius, Mask);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<BaseEntity>())
            {
                BaseEntity entity = cols[i].GetComponent<BaseEntity>();
                float sign = Mathf.Sign(entity.transform.position.x - transform.position.x);

                entity.Hurt(Damage, transform.position, sign >= 0 ? 1 : -1);
                entity.OnFire();
                if (entity.CurrentHealth > 0)
                {
                    entity.HitBack(sign >= 0 ? HitForce.x : -HitForce.x, HitForce.y, 0.1f);
                }


            }
            else if (cols[i].GetComponent<Player>())
            {
                Player player = cols[i].GetComponent<Player>();

                float sign = Mathf.Sign(player.transform.position.x - transform.position.x);
                player.Hurt(Damage, sign >= 0 ? true : false);
                player.HitBack(sign >= 0 ? HitForce.x : -HitForce.x, HitForce.y, 0.1f);

            }
            else if (cols[i].GetComponent<OilCan>() && this.transform != cols[i].transform)
            {

                cols[i].GetComponent<OilCan>().Hurt(Damage);
            }
            else if (cols[i].GetComponent<PoisongasBottle>())
            {
                cols[i].GetComponent<PoisongasBottle>().Hurt(Damage);
            }
        }
    }
}
