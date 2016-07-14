using UnityEngine;
using System.Collections;

public class PrisonCell : InteractiveItem
{
    public GameObject CurrentHostage;
 
    public override void Hurt(float damage)
    {
        StartCoroutine("HurtColorChange");
        
        Health -= damage;

        if (Health <= 0)
        {
            Director.Instance.Player.SaveHostage();

            if (DestroyEffect != null)
            {
                Instantiate(DestroyEffect, this.transform.position, Quaternion.identity);
            }
           
            this.transform.DetachChildren();
            CurrentHostage.GetComponent<Hostage>().Disappear();
            base.addFragment();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            BaseBullet bullet = collider.gameObject.transform.GetComponent<BaseBullet>();
            bullet.DestoryWithEffect();
            if (bullet.Owner == Util.ObjectOwner.PLAYER)
            {
                Hurt(bullet.Damage);
            }
        }
    }
}
