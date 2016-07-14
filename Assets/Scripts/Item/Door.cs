using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float Distance;
    public float Speed;
    

    private Vector3 currentPosition;

    void Start()
    {
        currentPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collider.gameObject.transform.GetComponent<Player>();
            if (player.HasKey)
            {
                player.HasKey = false;
                BaseUI.Instance.HideKey();
                Destroy(gameObject);
                //StartCoroutine(move());
            }
            
        }
    }

    IEnumerator move()
    {

        
        while ((transform.position.y - currentPosition.y) <= Distance)
        {
            transform.Translate(Vector3.up * Time.deltaTime * Speed);
            yield return null;
        }
        
        
    }
}
