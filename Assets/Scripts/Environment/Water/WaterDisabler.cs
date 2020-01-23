using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WaterDisabler : MonoBehaviour
{

    
    [SerializeField]
    private GameObject reflectionCamera;
    [SerializeField]
    private GameObject postProcessVolume;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DepthOfField depthOfField = null;
            postProcessVolume.GetComponent<PostProcessVolume>().profile.TryGetSettings(out depthOfField);
            if (depthOfField != null)
            {
                depthOfField.active = true;
            }
            reflectionCamera.SetActive(false);
        }
    }
}
