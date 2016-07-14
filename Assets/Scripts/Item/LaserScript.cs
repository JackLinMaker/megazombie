using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
    LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        //line.SetWidth(0.1f, 0.1f);
    }

    void Update()
    {
        //line.renderer.material.mainTextureScale = new Vector2(1.0f, 1.0f);
       
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, 20.0f);
        if(hit.collider != null)
        {
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, new Vector3(this.transform.position.x, this.transform.position.y - 20.0f, this.transform.position.z));
        }
        
    }
}
