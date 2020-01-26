using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [SerializeField]
    private GameObject fracturedVersion;

    private void OnMouseDown()
    {
        Instantiate(fracturedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
