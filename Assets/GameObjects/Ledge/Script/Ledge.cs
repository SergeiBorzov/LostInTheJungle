using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    public Vector3 ledgeOffset;

    [SerializeField]
    public Vector3 endPoint;

    [SerializeField]
    public Vector3 hangOffset;

    [SerializeField]
    public Transform next;

    [SerializeField]
    public Transform previous;
}
