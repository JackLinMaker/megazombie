using UnityEngine;
using System.Collections;

public class AK47 : BaseGun
{
    public float AutoShootingAccuracy;
    public override void ShootBullet(Transform host, bool isFacingRight, Transform point, Util.ObjectOwner owner, bool isShooting, float offset, bool IsAutomaticShooting)
    {
        this.shootPoint = point;
        this.host = host;
        this.owner = owner;
        if (isShooting)
        {
           
            GameObject bulletInstance = Instantiate(Bullet, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0.0f : 180.0f))) as GameObject;
            BaseBullet bullet = bulletInstance.GetComponent<BaseBullet>();
           

            if (bullet != null)
            {
                bullet.Speed = Speed;
                bullet.Damage = Damage;
                bullet.HitForceX = HitForceX;
                bullet.HitForceY = HitForceY;
                bullet.IsFacingRight = isFacingRight;
                bullet.Host = host;
                bullet.Range = Range + offset;
                bullet.Owner = owner;
                if (IsAutomaticShooting)
                {
                    bullet.Direction = Quaternion.Euler(0, 0, Random.Range(-AutoShootingAccuracy, AutoShootingAccuracy)) * Vector3.right;
                }
                else
                {
                    bullet.Direction = Quaternion.Euler(0, 0, Random.Range(-Accuracy, Accuracy)) * Vector3.right;
                }
               
                bullet.Name = "AK47";
            }
               
            
            
        }
        
        addShootEffect(isFacingRight);
        addShellCase(isFacingRight);

    }

    public override IEnumerator Reload()
    {
        return base.Reload();
    }
   
	
}
