using UnityEngine;
using System.Collections;

public class SwingSaw : MonoBehaviour
{
    public float Speed;
    
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * Speed);
    }
}
