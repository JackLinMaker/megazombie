using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour 
{
    public float FallingTime = 2.0f;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void ControllerEnter2D(CharacterController2D controller)
    {
        StartCoroutine(FallingCo());
    }

    IEnumerator FallingCo()
    {
        yield return new WaitForSeconds(FallingTime);
        rigidbody.gravityScale = 1.0f;
        boxCollider.enabled = false;
        Destroy(gameObject, 5.0f);
    }
	
}
