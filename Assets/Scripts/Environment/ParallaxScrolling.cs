using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour
{
  
    public GameObject Background;
    public float Smoothing = 1f;
    public float SpeedFactor = 2.0f;

    private float[] paralaxScales;
    private Transform camera;
    private Vector3 previousCamPos;


    void Awake()
    {
        camera = Camera.main.transform;

    }

    void Start()
    {
        previousCamPos = camera.position;
    }

    void Update()
    {

        for (int i = 0; i < Background.transform.childCount; i++)
        {
            float parallaxX = (previousCamPos.x - camera.position.x) * i * -10;
            float backgroundTargetPosX = Background.transform.GetChild(i).position.x - parallaxX;

            float parallaxY = (previousCamPos.y - camera.position.y) * i * -3;
            float backgroundTargetPosY = Background.transform.GetChild(i).position.y - parallaxY;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, Background.transform.GetChild(i).position.z);
            Background.transform.GetChild(i).position = Vector3.Lerp(Background.transform.GetChild(i).position, backgroundTargetPos, Smoothing * Time.deltaTime);
        }

        previousCamPos = camera.position;
    }

    void LateUpdate()
    {
        //Debug.Log("camera x =" + camera.position.x);
        //Background.transform.position = new Vector3(camera.position.x, camera.position.y, transform.position.z);
    }
}
