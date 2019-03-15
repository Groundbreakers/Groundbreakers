using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very simple camera shaker
/// </summary>

public class CameraShake : MonoBehaviour
{

    Vector3 cameraInitialPosition;
    public float shakeMagnetude, shakeTime;
    
    public void ShakeIt(float mag, float time)
    {
        this.shakeMagnetude = mag;
        this.shakeTime = time;
        cameraInitialPosition = this.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector3 cameraIntermadiatePosition = this.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        this.transform.position = cameraIntermadiatePosition;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        this.transform.position = cameraInitialPosition;
    }

}
