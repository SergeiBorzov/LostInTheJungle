using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
    }
}
