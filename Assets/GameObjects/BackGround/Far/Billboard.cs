using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private float offset;

    private void Awake()
    {
        float aspect = Camera.main.aspect;


        transform.localScale = new Vector3(2.4f*offset, offset, 1);
    }
 
    void Update()
    {
        Vector3 pos = Camera.main.transform.position;
        transform.position = new Vector3(pos.x, pos.y, offset);
        transform.forward = Camera.main.transform.forward;
    }
}
