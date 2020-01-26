using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRender : MonoBehaviour
{
    [SerializeField]
    GameObject reflectionCamera = null;
    [SerializeField]
    public GameObject postProcessVolume;

    private void OnPostRender()
    {
        
        if (reflectionCamera.activeSelf)
        {
            reflectionCamera.GetComponent<ReflectionCamera>().RenderReflection();
        }

    }
}
