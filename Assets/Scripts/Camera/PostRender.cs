using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRender : MonoBehaviour
{
    [SerializeField]
    ReflectionCamera reflectionCameraScript = null;


    private void OnPostRender()
    {
        reflectionCameraScript.RenderReflection();
    }
}
