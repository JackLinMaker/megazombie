using UnityEngine;
using System.Collections;

public class MacheteLight : MonoBehaviour
{
    public float StartRotationZ;
    public float EndRotationZ;
    public bool IsFaceRight = true;
    private float z;
    // Use this for initialization
    void Start()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, IsFaceRight ? 0 : 180, StartRotationZ));
        z = StartRotationZ;
    }

    void Update()
    {
        z = Mathf.MoveTowards(z, EndRotationZ, 20f);
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, IsFaceRight ? 0 : 180, z));
        if (z == EndRotationZ)
        {
            Destroy(this.gameObject);
        }
    }

}
