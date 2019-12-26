using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private GameObject startPoint = null;
    [SerializeField]
    private GameObject endPoint = null;

    private Transform characterTransform = null;


    private CapsuleCollider capsuleCollider;
    private void UpdateColliderTransform()
    {
        Vector3 up = startPoint.transform.position - endPoint.transform.position;
        up.Normalize();
        transform.up = up;
        transform.position = (startPoint.transform.position + endPoint.transform.position) / 2.0f;


        capsuleCollider.height = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);

    }

    private void Start()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        UpdateColliderTransform();

    }

    public void CreateEndPoint(Transform character)
    {
        characterTransform = character;


        var cableComponent = startPoint.GetComponent<CableComponent>();

        var characterCable = characterTransform.GetComponent<CableComponent>();
        characterCable.SetEndPoint(cableComponent.GetEndPoint());
        characterCable.SetMaterial(cableComponent.GetMaterial());

        var characterRb = characterTransform.gameObject.AddComponent<Rigidbody>();
        var characterHingeJoint = characterTransform.gameObject.AddComponent<HingeJoint>();
        characterHingeJoint.axis = new Vector3(-1.0f, 0.0f, 0.0f);
        characterHingeJoint.anchor = characterTransform.InverseTransformPoint(startPoint.transform.position);
        characterHingeJoint.connectedBody = startPoint.GetComponent<Rigidbody>();
       // var characterLine = characterTransform.gameObject.GetComponent<LineRenderer>();
        //characterLine.enabled = true;

        var endPointHinge = endPoint.GetComponent<HingeJoint>();
        endPointHinge.anchor = endPoint.transform.InverseTransformPoint(characterTransform.position);
        endPointHinge.connectedBody = characterRb;
        endPointHinge.useLimits = true;

        var endPointCollider = endPoint.GetComponent<SphereCollider>();
        endPointCollider.enabled = false;




        characterCable.enabled = true;


        cableComponent.BindEndPoint(characterTransform);

        //endPoint = transform.gameObject;

    }

    public void SetDefaultEndPoint(Transform character)
    {
        var cableComponent = startPoint.GetComponent<CableComponent>();


        characterTransform = character;
        var characterCable = characterTransform.GetComponent<CableComponent>();
        characterCable.enabled = false;
        var characterRb = characterTransform.gameObject.GetComponent<Rigidbody>();
        var characterHingeJoint = characterTransform.gameObject.GetComponent<HingeJoint>();
        var characterLine = characterTransform.gameObject.GetComponent<LineRenderer>();
        characterLine.enabled = false;

       
        Destroy(characterHingeJoint);
        Destroy(characterRb);




        var endPointHinge = endPoint.GetComponent<HingeJoint>();
        endPointHinge.anchor = endPoint.transform.InverseTransformPoint(startPoint.transform.position);
        endPointHinge.connectedBody = startPoint.GetComponent<Rigidbody>();
        endPointHinge.useLimits = false;

        var endPointCollider = endPoint.GetComponent<SphereCollider>();
        endPointCollider.enabled = true;

        var endPointScript = endPoint.GetComponent<EndPoint>();
        endPointScript.ResetPosition();

        cableComponent.BindEndPoint(endPoint.transform);
    }

    private void Update()
    {
        UpdateColliderTransform();
    }

}
