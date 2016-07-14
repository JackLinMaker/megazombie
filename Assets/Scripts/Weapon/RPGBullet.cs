using UnityEngine;
using System.Collections;

public class RPGBullet : BaseBullet
{
    public float BombDamage;

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {

            DestoryWithEffect();
        }
    }

    public override void DestoryWithEffect()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, 1);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<BaseEntity>())
            {
                BaseEntity entity = cols[i].GetComponent<BaseEntity>();
                entity.Hurt(BombDamage, transform.position, 1);
            }
        }

        distance = 0.0f;

        Transform desEffect = Instantiate(DestoryEffect, this.transform.position, Quaternion.identity) as Transform;
        desEffect.localRotation = Quaternion.Euler(new Vector3(0, 0, IsFacingRight ? 0 : 0));
        Destroy(gameObject);
    }

}
