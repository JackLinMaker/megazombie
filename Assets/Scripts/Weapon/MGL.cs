using UnityEngine;
using System.Collections;

public class MGL : BaseGun
{
    public Transform RPGBullet1;
    private Transform bulletinweapon;

    public override void Init(Transform gunShootPoint)
    {
        bulletinweapon = Instantiate(RPGBullet1) as Transform;
        bulletinweapon.parent = gunShootPoint.transform;
        bulletinweapon.localPosition = new Vector3(0, 0, 0);
        bulletinweapon.gameObject.SetActive(true);
    }

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
                bullet.Range = Range;
                bullet.Owner = owner;
                bullet.Host = host;
                bullet.Direction = Quaternion.Euler(0, 0, Random.Range(-Accuracy, Accuracy)) * Vector3.right;
                bullet.Name = "MGL";
            }
        }
        bulletinweapon.gameObject.SetActive(false);
        addShootEffect(isFacingRight);
        addShellCase(isFacingRight);
    }

    public override IEnumerator Reload()
    {
        yield return new WaitForSeconds(Rate - 0.1f);
        bulletinweapon.gameObject.SetActive(true);
    }
}
