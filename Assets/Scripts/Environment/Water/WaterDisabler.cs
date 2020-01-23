using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisabler : MonoBehaviour
{

    
    [SerializeField]
    private GameObject reflectionCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Not working!");
            reflectionCamera.SetActive(false);
        }
    }
}
