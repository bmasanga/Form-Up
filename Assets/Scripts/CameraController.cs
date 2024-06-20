using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CameraController : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] GameObject background;
    [SerializeField] [Range(0f, 1f)] float parallaxScale = 1.0f;

    [Header("Camera Shake Settings")]
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = .5f;
    
    float shakeTimer;
    Vector3 initialPosition;
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin shakeSettings;
    
     void Awake()
     {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
     }
     
     void Start()
    {
        initialPosition = transform.position;
        StopShake();
    }
    
    void FixedUpdate()
    {
        Parallax();
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }

    void Parallax()
    {
        background.transform.position = new Vector3(-transform.position.x * parallaxScale, -transform.position.y * parallaxScale, background.transform.position.z);
    }
    
    public void ShakeCamera()
    {
        shakeSettings = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakeSettings.m_AmplitudeGain = shakeMagnitude;
        shakeTimer = shakeDuration;
    }

    void StopShake()
    {
        shakeSettings = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakeSettings.m_AmplitudeGain = 0f;
        shakeTimer = 0f;
    }

    // public void Play()
    // {
    //     StartCoroutine(Shake());
    // }

    // IEnumerator Shake()
    // {
    //     float elapsedTime = 0;
    //     while(elapsedTime < shakeDuration)
    //     {
    //         transform.position = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
    //         elapsedTime += Time.deltaTime;
    //         yield return new WaitForEndOfFrame();
    //     }
    //     transform.position = initialPosition;

    //}

}
