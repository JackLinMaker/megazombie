using UnityEngine;
using System.Collections;

public class BaseGun : MonoBehaviour
{
    #region property

    public string Name;
    public float Range;
    public float Rate;
    public float Damage;
    public float HitForceX;
    public float HitForceY;
    public float Speed;
    public int IsAutomatic;
    public float Accuracy;
    public Material GunMaterial;
    public int GunLenth;
    public AudioClip ShootSound;

    #endregion

    #region component
    public GameObject Bullet;
    public GameObject ShootEffect;
    public Rigidbody2D ShellCase;
    #endregion

    protected Transform host;
    protected Transform shootPoint;
    protected Util.ObjectOwner owner;

    void Awake()
    {
    }

    public virtual void Init(Transform gunShootPoint)
    {

    }

    public virtual void ShootBullet(Transform host, bool isFacingRight, Transform point, Util.ObjectOwner owner, bool isShooting, float offset, bool IsAutomaticShooting)
    {

    }

    public virtual IEnumerator Reload()
    {
        yield return null;
    }

    protected void addShootEffect(bool isFacingRight)
    {
        if (ShootEffect != null)
        {
            GameObject shootEffect = Instantiate(ShootEffect, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, isFacingRight ? 0 : 180.0f))) as GameObject;
            shootEffect.transform.parent = host;
        }

    }

    protected void addShellCase(bool isFacingRight)
    {
        if (ShellCase != null)
        {
            Vector3 shellPos = shootPoint.position;
            Rigidbody2D shellCase = Instantiate(ShellCase, shellPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0.0f, 180.0f)))) as Rigidbody2D;
            shellCase.velocity = new Vector2(isFacingRight ? Random.Range(-4.0f, -2.0f) : Random.Range(2.0f, 4.0f), Random.Range(2.0f, 4.0f));
            shellCase.angularVelocity = 360.0f;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            collider.gameObject.GetComponent<Player>().PickupGun(this);
            this.gameObject.SetActive(false);
        }

    }

}
