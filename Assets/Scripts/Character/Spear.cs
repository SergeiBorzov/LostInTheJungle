using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField]
    private Transform spineTransform;
    [SerializeField]
    private Transform rightHandHold;
    [SerializeField]
    private Transform spearTransform;
    [SerializeField]
    private SpearScript spearScript;

    private CharacterIK characterIKscript;
   

    private Transform originalTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;


    private void Start()
    {
        characterIKscript = GetComponent<CharacterIK>();
    }
    private void PickSpear()
    {
        originalPosition = spearTransform.localPosition;
        originalRotation = spearTransform.localRotation;
        spearTransform.SetParent(rightHandHold);
    }

    public void RemoveSpear()
    {
        spearTransform.SetParent(spineTransform);
        spearTransform.localPosition = originalPosition;
        spearTransform.localRotation = originalRotation;
    }


    private IEnumerator SpearPhysics(Vector3 direction, Transform target)
    {
        float spearSpeed = 3.0f;
        spearTransform.forward = direction;

        
        while (!spearScript.GetCollisionStatus())
        {
            spearTransform.position += direction * spearSpeed * Time.deltaTime;
            yield return null;
        }

        spearTransform.SetParent(target);
    

        yield return null;
    }

    private void ThrowSpear()
    {
        spearTransform.parent = null;

        Transform target = characterIKscript.GetTarget();
        Vector3 direction = target.position - rightHandHold.position;
        IEnumerator coroutine = SpearPhysics(direction, target);
        StartCoroutine(coroutine);

        characterIKscript.ReleaseTarget();



    }

}
