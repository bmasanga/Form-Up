using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : MonoBehaviour
{
    [SerializeField] AIData aIData;
    [SerializeField] List<Detector> detectors;
    [SerializeField] float detectionDelay = 0.05f;
    // [SerializeField] float aiUpdateDelay = 0.06f;

    void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aIData);
        }
    }
}
