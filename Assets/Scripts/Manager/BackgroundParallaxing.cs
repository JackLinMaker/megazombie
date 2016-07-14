using UnityEngine;
using System.Collections;

public class BackgroundParallaxing : MonoBehaviour
{
    public Transform[] Backgrounds;
    public float ParallaxScale;
    public float ParallaxReductionFactor;
    public float Smoothing;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

    }

    void Update()
    {
        if (Backgrounds.Length <= 0) return;

        float parallax = -(lastPosition.x - transform.position.x) * ParallaxScale;
        for (int i = 0; i < Backgrounds.Length; i++)
        {
            float backgroundTargetPosition;
            if (i == 2)
            {
                backgroundTargetPosition = Backgrounds[i].position.x - parallax * (0.8f * ParallaxReductionFactor);
            }
            else if (i == 1)
            {
                backgroundTargetPosition = Backgrounds[i].position.x - parallax * (3f * ParallaxReductionFactor);
            }
            else
            {
                backgroundTargetPosition = Backgrounds[i].position.x - parallax * (i * ParallaxReductionFactor);
            }

            Backgrounds[i].position = Vector3.Lerp(
                Backgrounds[i].position,
                new Vector3(backgroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z),
                Smoothing * Time.deltaTime);

        }
        lastPosition = transform.position;
    }
}
