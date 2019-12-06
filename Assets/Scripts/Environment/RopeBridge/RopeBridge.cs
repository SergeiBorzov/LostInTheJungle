using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridge : MonoBehaviour
{
   

    [SerializeField]
    private GameObject bridgePlank;

    [SerializeField]
    private uint planksNumber;

    [SerializeField]
    private float plankDistance;

    [SerializeField]
    private Vector3 plankScale;

    private List<GameObject> planks;

    private bool bridgeBuilded = false;
    public void BuildBridge()
    {
        if (!bridgeBuilded)
        {
            planks = new List<GameObject>();
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
                    hingeJoint.connectedBody = planks[i - 1].GetComponent<Rigidbody>();
                }

                if (i == planksNumber - 1)
                {
                    planks[i].AddComponent<HingeJoint>();
                }

                
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
