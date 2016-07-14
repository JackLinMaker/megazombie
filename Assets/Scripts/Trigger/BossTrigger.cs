using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{

    public BaseEntity Boss;
    private Player player;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            player = col.transform.GetComponent<Player>();
            if (player.CurrentHealth <= 0) return;
            Camera.main.GetComponent<CameraController>().StartBossMode();
            Boss.GetComponent<Perspective>().Detected = true;
            Boss.GetComponent<BaseEntity>().Target = col.gameObject.transform;
        }
    }

    void Update()
    {
        if (Boss.CurrentHealth <= 0)
        {
            Camera.main.GetComponent<CameraController>().StopBossMode();
            Destroy(this.gameObject);
        }

        if (player)
        {
            if (player.CurrentHealth <= 0)
            {
                Camera.main.GetComponent<CameraController>().StopBossMode();
                this.GetComponent<Collider2D>().enabled = true;
            }
            
        }
      
    }
}
