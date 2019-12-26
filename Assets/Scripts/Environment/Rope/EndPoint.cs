using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    
    [SerializeField]
    private Transform startPoint;

    private HingeJoint hinge;
    private Rigidbody rigidBody = null;

    public void ResetPosition()
    {
        rigidBody.isKinematic = true;
        var cableComponent = startPoint.GetComponent<CableComponent>();
        transform.position = startPoint.transform.position - transform.up * cableComponent.GetLength();
        hinge.anchor = transform.InverseTransformPoint(startPoint.transform.position - new Vector3(0.0f, 0.6f, 0.0f));
        rigidBody.isKinematic = false;
    }

    void Start()
    {
        var cableComponent = startPoint.GetComponent<CableComponent>();
        transform.position = startPoint.transform.position - transform.up * cableComponent.GetLength();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        hinge = gameObject.GetComponent<HingeJoint>();
        hinge.anchor = transform.InverseTransformPoint(startPoint.transform.position - new Vector3(0.0f, 0.6f, 0.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
