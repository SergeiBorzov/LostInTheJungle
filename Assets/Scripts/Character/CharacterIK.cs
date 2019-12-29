using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    private Animator animator = null;


    [SerializeField]
    private Transform targetTransform = null;


    public void SetTarget(Transform newTransform)
    {
        targetTransform = newTransform;
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(0.6f, 0.6f, 0.6f);
        animator.SetLookAtPosition(targetTransform.position);
    }
}
