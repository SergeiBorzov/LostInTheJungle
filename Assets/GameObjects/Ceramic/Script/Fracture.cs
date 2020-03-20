using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [SerializeField]
    private GameObject fracturedVersion;
    /*
    public void Action()
    {
        Instantiate(fracturedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    */
    public void Action()
    {
        var go = Instantiate(fracturedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(go, 3.0f);
    }
    
}
