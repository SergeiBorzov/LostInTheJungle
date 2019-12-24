using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private float ropeHeight = 2.0f;
    [SerializeField]
    private float ropeRadius = 0.5f;
    [SerializeField]
    private float ropeMaxAngle = 30.0f;
    [SerializeField]
    private Material ropeMaterial;
    [SerializeField]
    private int ropeResolution = 10;

    private bool ropeBuilded = false;

    private LineRenderer lineRenderer;

    // Refactor!
    private GameObject rope;
    private CapsuleCollider ropeCollider;
    //

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawRope();
    }


    private void DrawRope()
    {
        Vector3[] ropePoints = new Vector3[5];


        List<CapsuleCollider> segmentTransforms = new List<CapsuleCollider>(gameObject.GetComponentsInChildren<CapsuleCollider>());

        for (int i = 0; i < 4; i++)
        {            
            if (i == 0)
            {
                ropePoints[0] = segmentTransforms[i].transform.position + segmentTransforms[i].transform.up * (ropeHeight / 8.0f);
            }
            ropePoints[i + 1] = segmentTransforms[i].transform.position - segmentTransforms[i].transform.up * (ropeHeight / 8.0f);
        }

        List<Vector3> curvePositions = new List<Vector3>();
        for (int i = 1; i <= ropeResolution; i++)
        {
            float t = 0.0f + i * 1.0f / ropeResolution;
            curvePositions.Add(DeCasteljau(ropePoints, t));
        }

        lineRenderer.positionCount = curvePositions.Count;
        lineRenderer.SetPositions(curvePositions.ToArray());


    }

    private Vector3 DeCasteljau(Vector3[] points, float t)
    {
        float oneMinusT = 1f - t;

        for (int j = 1; j <= 4; j++)
        {
            for (int i = 0; i < points.Length - j; i++)
            {
                points[i] = oneMinusT * points[i] + t * points[i + 1];
            }
        }

        return points[0];
    }

    public CapsuleCollider GiveCurrentSegment(Vector3 worldSpacePosition)
    {
        CapsuleCollider[] colliders = gameObject.GetComponentsInChildren<CapsuleCollider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if( worldSpacePosition.y > (colliders[i].transform.position - (colliders[i].transform.up*ropeHeight/8.0f)).y
                && worldSpacePosition.y < (colliders[i].transform.position + (colliders[i].transform.up * ropeHeight / 8.0f)).y)
            {
                return colliders[i];
            }
        }

        return null;
    }

    public void CreateRope()
    {

        if (!ropeBuilded)
        {
            GameObject[] segments = new GameObject[4];


            for (int i = 0; i < 4; i++)
            {
                segments[i] = new GameObject("Segment" + i);
                segments[i].transform.SetParent(transform);
                segments[i].transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - ropeHeight / 8.0f - ropeHeight * i / 4.0f, 0.0f);
                segments[i].tag = "Rope";

                // Adding Collider
                CapsuleCollider capsuleCollider = segments[i].AddComponent<CapsuleCollider>();
                capsuleCollider.radius = ropeRadius;
                capsuleCollider.height = ropeHeight/4.0f;

                // Adding Rigidbody
                var tensionedRopeRb = segments[i].AddComponent<Rigidbody>();
                tensionedRopeRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

                // Adding HingeJoint
                HingeJoint hingeJoint = segments[i].AddComponent<HingeJoint>();
                hingeJoint.axis = Vector3.forward;
                hingeJoint.anchor = new Vector3(0.0f, ropeHeight / 8.0f, 0.0f);
                hingeJoint.useLimits = true;
                hingeJoint.enablePreprocessing = false;
                hingeJoint.limits = new JointLimits
                {
                    min = -ropeMaxAngle,
                    max = ropeMaxAngle
                };

                if (i == 0)
                {
                    continue;
                }
                else
                {
                    hingeJoint.connectedBody = segments[i - 1].GetComponent<Rigidbody>();
                }

               
            }

            // Adding lineRenderer
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = ropeMaterial;
            lineRenderer.startWidth = ropeRadius;
            lineRenderer.endWidth = ropeRadius;
            lineRenderer.numCapVertices = 5;
            ropeBuilded = true;
            DrawRope();
        }
    }

     
   /* public void SetTensionHeight(Vector3 hitPoint)
    {

        var tensionedPart = transform.Find("Segment0").gameObject;
        var tensionedCollider = tensionedPart.GetComponent<CapsuleCollider>();

        var topPosition = tensionedCollider.center + new Vector3(0.0f, tensionedCollider.height / 2.0f, 0.0f);
        var hitPosition = tensionedCollider.transform.InverseTransformPoint(hitPoint);
        float length = Vector3.Distance(topPosition, hitPosition);

        //tensionedPart.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - length / 2.0f, 0.0f);
        tensionedPart.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - length, 0.0f);
        //tensionedCollider.center = new Vector3(0.0f, ropeHeight/2.0f - length, 0.0f);
        tensionedCollider.height = length;

        var hingeJoint = tensionedPart.GetComponent<HingeJoint>();
        hingeJoint.anchor = new Vector3(0.0f, length / 2.0f, 0.0f);


        //tensionedPart.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - tensionedPartLength/2.0f, 0.0f);

        // Other colliders!
        for (int i = 1; i <= 3; i++)
         {
            
            var tailPart = transform.Find("Segment" + i).gameObject;
           
            var tailCollider = tailPart.GetComponent<CapsuleCollider>();
            var tailJoint = tailPart.GetComponent<HingeJoint>();
            tailCollider.enabled = false;
            //float colliderLength = (ropeHeight - length) / 3.0f;
           //tailJoint.anchor = new Vector3(0.0f, colliderLength, 0.0f);

            //tailPart.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - length - colliderLength / 2.0f - (i-1) * colliderLength);
           // tailCollider.height = colliderLength;

           /* if (i > 1)
            {
                
            }
            else
            {
                //tailPart.transform.localPosition = hitPosition;
                Debug.Log(hitPosition);
                //tailPart.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f - length - colliderLength/2.0f - (i-1)*colliderLength, 0.0f);
                //tailCollider.center = tailPart.transform.localPosition - new Vector3(0.0f, colliderLength/2.0f + (i-1) * colliderLength, 0.0f);
                //tailCollider.center = new Vector3(0.0f, colliderLe;
                
                //tailCollider.center = hitPosition;
            }

           

           

         }




    }*/


    /*private Rigidbody CreateTop()
    {
        GameObject top = new GameObject("Top");
        top.transform.SetParent(transform);
        top.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f, 0.0f);

        var topRigidbody = top.AddComponent<Rigidbody>();
        topRigidbody.isKinematic = true;

        return topRigidbody;
    }*/

    

    public void DestroyRope()
    {
       
        DestroyImmediate(gameObject.GetComponent<LineRenderer>());

        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }

        foreach (GameObject go in list)
        {
            DestroyImmediate(go);
        }

        ropeBuilded = false;
        
    }
}
