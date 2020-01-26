using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    private Animator animator = null;
    private Transform targetTransform = null;


    public void SetTarget(Transform newTransform)
    {
        targetTransform = newTransform;
    }

    public Transform GetTarget()
    {
        return targetTransform;
    }

    public void ReleaseTarget()
    {
        targetTransform = null;
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (targetTransform != null)
        {
            animator.SetLookAtWeight(0.6f, 0.6f, 0.6f);
            animator.SetLookAtPosition(targetTransform.position);
        }
        
    }
}
