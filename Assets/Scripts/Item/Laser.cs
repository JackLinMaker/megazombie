using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    public GameObject LaserMiddle;
    public GameObject LaserDevice;

    public Vector2 Direction;

    public float MaxLaserSize = 20.0f;

    private GameObject middle;
   
    

    void Update()
    {
        if (middle == null)
        {
            if (Direction == Vector2.up || Direction == -Vector2.up)
            {

                middle = Instantiate(LaserMiddle) as GameObject;
                middle.transform.parent = this.transform;
                middle.transform.localEulerAngles = Vector2.zero;
                middle.transform.localPosition = Vector3.zero;
            }
            else if (Direction == Vector2.right || Direction == -Vector2.right)
            {
                middle = Instantiate(LaserMiddle) as GameObject;
                middle.transform.parent = this.transform;
                middle.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                middle.transform.localPosition = Vector3.zero;
            }

       

           
         
        }

        
        float currentLaserSize = MaxLaserSize;

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Direction, MaxLaserSize);

        if (hit.collider != null)
        {
            currentLaserSize = Vector2.Distance(hit.point, this.transform.position);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player player = hit.collider.gameObject.transform.GetComponent<Player>();
                if (player.CurrentHealth > 0)
                {
                    //player.InstantKill();
                }
            }
            
            
        }

        if (Direction == Vector2.up)
        {
            LaserDevice.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize, middle.transform.localScale.z);
            middle.transform.localPosition = new Vector2(0f, (currentLaserSize / 2f));
        }
        else if (Direction == -Vector2.up)
        {
            LaserDevice.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -180.0f);
            middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize, middle.transform.localScale.z);
            middle.transform.localPosition = new Vector2(0f, -(currentLaserSize / 2f));
        }
        else if (Direction == Vector2.right)
        {
            LaserDevice.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
            middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize, middle.transform.localScale.z);
            middle.transform.localPosition = new Vector2((currentLaserSize / 2f), 0.0f);
        }
        else if (Direction == -Vector2.right)
        {
            LaserDevice.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize, middle.transform.localScale.z);
            middle.transform.localPosition = new Vector2(-(currentLaserSize / 2f), 0.0f);
        }

      

    }
	
}
