﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Material waterMaterial;

    [SerializeField]
    private Transform waterTransform;

    [SerializeField]
    private RenderTexture reflectionTexture;
  
    private Camera reflectionCamera;
    void Start()
    {
        reflectionCamera = gameObject.AddComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    private void OnPostRender()
    {
        RenderReflection();
    }

    private void RenderReflection()
    {
        Camera mainCamera = Camera.main;
        reflectionCamera.CopyFrom(mainCamera);

        Vector3 cameraDirectionWS = mainCamera.transform.forward;
        Vector3 cameraUpWS = mainCamera.transform.up;
        Vector3 cameraPositionWS = mainCamera.transform.position;

        Vector3 cameraDirectionPS = waterTransform.InverseTransformDirection(cameraDirectionWS);
        Vector3 cameraUpPS = waterTransform.InverseTransformDirection(cameraUpWS);
        Vector3 cameraPositionPS = waterTransform.InverseTransformPoint(cameraPositionWS);

        cameraDirectionPS.y *= -1;
        cameraUpPS.y *= -1;
        cameraPositionPS.y *= -1;

        cameraDirectionWS = waterTransform.TransformDirection(cameraDirectionPS);
        cameraUpWS = waterTransform.TransformDirection(cameraUpPS);
        cameraPositionWS = waterTransform.TransformPoint(cameraPositionPS);


        reflectionCamera.transform.position = cameraPositionWS;
        reflectionCamera.transform.LookAt(cameraPositionWS + cameraDirectionWS, cameraUpWS);


        Vector4 clipPlaneWorldSpace = new Vector4(0, -1, 0, -Vector3.Dot(-Vector3.up, waterTransform.position));
        Vector4 clipPlaneCameraSpace =  Matrix4x4.Transpose(mainCamera.cameraToWorldMatrix) * clipPlaneWorldSpace;


        reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);

        reflectionCamera.targetTexture = reflectionTexture;
        reflectionCamera.Render();
    }

}
