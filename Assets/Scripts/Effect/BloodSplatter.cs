using UnityEngine;
using System.Collections;

public class BloodSplatter : MonoBehaviour
{
    public GameObject BloodDrip;

    void Awake()
    {
       
        int x = -1;
        int drops = Random.Range(3,5);
       
        while (x <= drops)
        {
            x++;
            Vector3 pos = transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.0f);
            GameObject drip = Instantiate(BloodDrip, pos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)))) as GameObject;

            var scaler = Random.value;
            Vector3 original = drip.transform.localScale;
            drip.transform.localScale = new Vector3(original.x * scaler, original.y * scaler, original.z);
            
                   
                
        }
    }
	
}
