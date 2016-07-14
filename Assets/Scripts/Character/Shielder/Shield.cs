using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    public SkinnedMeshRenderer NormalShield;
    public Shielder Entity;

    void Awake()
    {

    }
    public void OnTriggerEnter2D(Collider2D collider)
    {


        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet") && collider.gameObject.GetComponent<BaseBullet>().Owner == Util.ObjectOwner.PLAYER)
        {
           
            BaseBullet bullet = collider.gameObject.transform.GetComponent<BaseBullet>();
            bullet.DestoryWithEffect();
            StartCoroutine("HurtColorChange");
            if (Entity)
            {
                if (Entity.GetFSM().IsInState(ShielderPatrolState.Instance) || Entity.GetFSM().IsInState(ShielderPauseState.Instance))
                {
                    float distance = Mathf.Abs(Entity.Player.position.x - transform.position.x);
                    if (distance > Entity.AttackRange)
                    {
                        Entity.GetFSM().ChangeState(ShielderChaseState.Instance);
                    }
                    else
                    {
                        Entity.GetFSM().ChangeState(ShielderDefenseState.Instance);
                    }

                }
            }
        }
      
    }

    public void Hurt(float damage)
    {
        StartCoroutine("HurtColorChange");
        if (Entity)
        {
            if (Entity.GetFSM().IsInState(ShielderPatrolState.Instance) || Entity.GetFSM().IsInState(ShielderPauseState.Instance))
            {
                float distance = Mathf.Abs(Entity.Player.position.x - transform.position.x);
                if (distance > Entity.AttackRange)
                {
                    Entity.GetFSM().ChangeState(ShielderChaseState.Instance);
                }
                else
                {
                    Entity.GetFSM().ChangeState(ShielderDefenseState.Instance);
                }

            }
        }
    }

    private IEnumerator HurtColorChange()
    {
        NormalShield.materials[0].SetFloat("_FlashAmount", 0.5f);
        NormalShield.materials[0].SetColor("_FlashColor", Color.red);
        yield return new WaitForSeconds(0.08f);
        NormalShield.materials[0].SetFloat("_FlashAmount", 0.3f);
        yield return new WaitForSeconds(0.03f);
        NormalShield.materials[0].SetFloat("_FlashAmount", 0.15f);
        yield return new WaitForSeconds(0.03f);
        NormalShield.materials[0].SetFloat("_FlashAmount", 0f);

    }
}
