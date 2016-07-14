using UnityEngine;
using System.Collections;

[RequireComponent(typeof (SpriteRenderer))]
public class Tilling : MonoBehaviour 
{
    public int OffsetX = 2;
    public bool HasARightBuddy = false;
    public bool HasALeftBuddy = false;
    public bool ReverseScale = false;

    private float spriteWidth = 0f;
    private Camera camera;
    private Transform myTransform;

    void Awake()
    {
        camera = Camera.main;
        myTransform = transform;
    }

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        spriteWidth = renderer.sprite.bounds.size.x;
    }

    void Update()
    {
        if (HasALeftBuddy == false || HasARightBuddy == false)
        {
            float camHorizontalExtend = camera.orthographicSize * Screen.width / Screen.height;
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            if (camera.transform.position.x >= edgeVisiblePositionRight - OffsetX && HasARightBuddy == false)
            {
                MakeNewBuddy(1);
                HasARightBuddy = true;
            }
            else if (camera.transform.position.x <= edgeVisiblePositionLeft + OffsetX && HasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                HasALeftBuddy = true;
            }
        }
    }

    void MakeNewBuddy(int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;
        if (ReverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tilling>().HasALeftBuddy = true;
            
        }
        else
        {
            newBuddy.GetComponent<Tilling>().HasARightBuddy = true;
            
        }
    }
}
