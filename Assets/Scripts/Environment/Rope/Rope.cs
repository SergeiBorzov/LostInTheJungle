using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private float ropeHeight;
    [SerializeField]
    private float ropeRadius;
    [SerializeField]
    private float ropeMaxAngle;
    [SerializeField]
    private int ropeDrawPointsCount;

    private bool ropeBuilded = false;

    private LineRenderer lineRenderer;

    private GameObject rope;
    private CapsuleCollider ropeCollider;

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        rope = transform.Find("Tensioned rope").gameObject;
        ropeCollider = rope.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        DrawRope();
    }


    private void DrawRope()
    {
        List<Vector3> positions = new List<Vector3>();
        lineRenderer.positionCount = ropeDrawPointsCount;

        Vector3 ropeCenter = rope.transform.position + ropeCollider.center;

        for (int i = 0; i < ropeDrawPointsCount; i++)
        {
            positions.Add(ropeCenter - rope.transform.up * (ropeHeight / 2.0f) + i*rope.transform.up * ropeHeight / (float)ropeDrawPointsCount);
            positions.Add(ropeCenter + rope.transform.up * (ropeHeight / 2.0f));
        }
        lineRenderer.SetPositions(positions.ToArray());
    }


    public void CreateRope()
    {
        if (!ropeBuilded)
        {
            

            GameObject top = new GameObject("Top");
            top.transform.SetParent(transform);
            top.transform.localPosition = new Vector3(0.0f, ropeHeight / 2.0f, 0.0f);

            var topRigidbody = top.AddComponent<Rigidbody>();
            topRigidbody.isKinematic = true;
           

            GameObject tensionedRope = new GameObject("Tensioned rope");
            tensionedRope.transform.SetParent(transform);
            tensionedRope.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            tensionedRope.tag = "Rope";
            rope = tensionedRope;


            CapsuleCollider capsuleCollider = tensionedRope.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = ropeRadius;
            capsuleCollider.height = ropeHeight;
            ropeCollider = capsuleCollider;

            var tensionedRopeRb = tensionedRope.AddComponent<Rigidbody>();
            tensionedRopeRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

            HingeJoint hingeJoint = tensionedRope.AddComponent<HingeJoint>();
            hingeJoint.axis = Vector3.forward;
            hingeJoint.anchor = new Vector3(0.0f, ropeHeight/ 2.0f, 0.0f);
            hingeJoint.connectedBody = topRigidbody;
            hingeJoint.useLimits = true;
            hingeJoint.enablePreprocessing = false;
            hingeJoint.limits = new JointLimits
            {
                min = -ropeMaxAngle,
                max = ropeMaxAngle
            };

            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startWidth = ropeRadius;
            lineRenderer.endWidth = ropeRadius;

          


            ropeBuilded = true;
            DrawRope();
        }
    }

    public void DestroyRope()
    {
       
        DestroyImmediate(gameObject.GetComponent<BoxCollider>());
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
