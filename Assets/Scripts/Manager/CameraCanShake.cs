using UnityEngine;
using System.Collections;

public class CameraCanShake : MonoBehaviour
{
    public Camera MainCamera;
    public float shakeAmount;
    public Vector3 originalCameraPosition;

    private float index = 0;
    public bool isSimpleShake = false;
    public bool IsJumapShake = false;
    private Vector3 to;
    private CameraController cc;
    public void SimpleShake()
    {
        //isSimpleShake = true;
        //to = originalCameraPosition + new Vector3(0.0f, -0.5f, 0);
        //index = 0;
        shakeAmount = 0.2f;
        InvokeRepeating("startShake", 0, 0.05f);
        Invoke("stopShake", 0.1f);
    }

    public void JumpShake()
    {
        //IsJumapShake = true;
        //to = originalCameraPosition + new Vector3(0, -0.02f, 0);
        //index = 0;
        InvokeRepeating("startShake", 0, 0.05f);
        Invoke("stopShake", 0.3f);
    }

    private void startShake()
    {
        //time -= 0.01f;
        if (shakeAmount > 0)
        {
            float randowFactor = Random.value;
            float totalAmount = randowFactor * shakeAmount * 2 - shakeAmount;
        
            Vector3 pos = MainCamera.transform.position;
            pos.y += totalAmount;
            MainCamera.transform.position = pos;
        }
    }


    void Update()
    {
        if (isSimpleShake)
        {

            if (index == 0)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.6f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, 0.35f, 0);
                    index++;
                }
            }
            if (index == 1)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.6f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, -0.1f, 0);
                    index++;
                }
            }

            if (index == 2)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.6f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, 0.15f, 0);
                    index++;
                }
            }

            if (index == 3)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.6f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, -0.05f, 0);
                    index++;
                }
            }

            if (index == 4)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.7f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, 0.05f, 0);
                    index++;
                }
            }

            if (index == 5)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.7f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    index++;
                }
            }
            if (index == 6)
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, originalCameraPosition.y, MainCamera.transform.position.z);
                isSimpleShake = false;
            }
        }

        if (IsJumapShake)
        {
            if (index == 0)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.7f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition + new Vector3(0, 0.02f, 0); ;
                    index++;
                }
            }

            if (index == 1)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.4f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    to = originalCameraPosition;
                    index++;
                }
            }

            if (index == 2)
            {
                float posy = Mathf.Lerp(MainCamera.transform.position.y, to.y, 0.4f);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, posy, MainCamera.transform.position.z);
                if (Mathf.Abs(MainCamera.transform.position.y - to.y) < 0.005)
                {
                    index++;
                }
            }

            if (index == 2)
            {
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, originalCameraPosition.y, MainCamera.transform.position.z);
                IsJumapShake = false;
            }
        }
    }

    private void stopShake()
    {
        CancelInvoke("startShake");
        
    }
}
