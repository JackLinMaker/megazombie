using UnityEngine;
using System.Collections;

public class WaterAnimator : MonoBehaviour 
{
    public float TimePerLayer = 0.5f;

    private void Start()
    {
        Debug.Log("Start");
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        bool isLayer1Active = true;
        var layer1Objs = GameObject.FindGameObjectsWithTag("WaterLayer1");
        var layer2Objs = GameObject.FindGameObjectsWithTag("WaterLayer2");

        while (true)
        {
            // We take for granted these objects only have visuals that 
            // are being enabled and disabled
            foreach (var obj1 in layer1Objs)
            {
                obj1.SetActive(isLayer1Active);
            }
            foreach (var obj2 in layer2Objs)
            {
                obj2.SetActive(!isLayer1Active);
            }

            isLayer1Active = !isLayer1Active;
            yield return new WaitForSeconds(TimePerLayer);
        }
    }
	
}
