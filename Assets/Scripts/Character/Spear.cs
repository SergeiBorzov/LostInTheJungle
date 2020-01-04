using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField]
    private Transform rightHandHold;
    [SerializeField]
    private Transform spearTransform;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private void PickSpear()
    {
        originalPosition = spearTransform.localPosition;
        originalRotation = spearTransform.localRotation;
        spearTransform.SetParent(rightHandHold);
    }

    private void RemoveSpear()
    {
        spearTransform.SetParent(transform);
        spearTransform.localPosition = originalPosition;
        spearTransform.localRotation = originalRotation;
    }

}
