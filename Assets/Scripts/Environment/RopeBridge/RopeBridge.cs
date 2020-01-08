using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeBridge : MonoBehaviour
{
   

    [SerializeField]
    private GameObject bridgePlank = null;

    [SerializeField]
    private GameObject bridgeColumn = null;

    [SerializeField]
    private uint planksNumber = 0;

    [SerializeField]
    private float plankDistance = 0.0f;

    [SerializeField]
    private Vector3 plankScale = new Vector3(); 

    private bool bridgeBuilded = false;
    public void BuildBridge()
    {
        if (!bridgeBuilded)
        {
            List<GameObject> planks = new List<GameObject>();
            int middle = (int) planksNumber / 2;
            for (int i = 0; i < planksNumber; i++)
            {



                planks.Add(Instantiate(bridgePlank, transform.position + (plankScale.x + plankDistance) * i * Vector3.right, Quaternion.identity));
                planks[i].transform.localScale = plankScale;
                planks[i].transform.SetParent(transform);
                planks[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;

                if (i != 0)
                {
                    HingeJoint hingeJoint = planks[i].GetComponent<HingeJoint>();
                    hingeJoint.anchor = new Vector3(0.5f, 0.5f, 0);
                    hingeJoint.connectedBody = planks[i - 1].GetComponent<Rigidbody>();
                    hingeJoint.useLimits = true;
                    hingeJoint.limits = new JointLimits
                    {
                        min = -5,
                        max = 5
                    };
                }

                if (i == planksNumber - 1)
                {
                    planks[i].AddComponent<HingeJoint>();
                }

                
            }

            int index = planks.Count;
            Vector3[] offsets = new Vector3[4];

            offsets[0] = -plankScale.x * Vector3.right - plankScale.z * Vector3.forward / 2.0f + Vector3.up + transform.position;
            offsets[1] = -plankScale.x * Vector3.right + plankScale.z * Vector3.forward / 2.0f + Vector3.up + transform.position;
            offsets[2] = plankScale.x * Vector3.right - plankScale.z * Vector3.forward / 2.0f + Vector3.up + planks[index - 1].transform.position;
            offsets[3] = plankScale.x * Vector3.right + plankScale.z * Vector3.forward / 2.0f + Vector3.up + planks[index - 1].transform.position;

            foreach(Vector3 offset in offsets)
            {
                GameObject go = Instantiate(bridgeColumn, offset, Quaternion.identity);
                go.transform.SetParent(transform);
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                index += 1;
            }
            
            bridgeBuilded = true;
        }
        
       
    }

    public void DestroyBridge()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }

        foreach(GameObject go in list)
        {
            DestroyImmediate(go);
        }

        bridgeBuilded = false;
    }
        
}
