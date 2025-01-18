using System.Collections;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class DayNightManager : MonoBehaviour
{
    public float DayCycleInSeconds = 120f;
    public float TimeMultiplier = 1f;
    public float DistanceMultiplier = 1f;

    public Light SunLight;

    public LensFlareComponentSRP SunFlare;
    public Transform CameraTransform;


    // Update is called once per frame
    void Update()
    {
        SunLight.transform.Rotate(Vector3.right, -(360f / DayCycleInSeconds) * Time.deltaTime * TimeMultiplier);
        SunFlare.transform.position = CameraTransform.position - SunLight.transform.forward * RuntimeTerrainGenerator.Instance.ViewDistance * DistanceMultiplier;
    }
}