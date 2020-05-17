using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform respawn;

    private void OnTriggerEnter(Collider other)
    {
        var characterScript = other.gameObject.GetComponent<Character>();
        if (characterScript != null)
        {
            Debug.Log("CheckPoint!");
            GameMaster.lastCheckPoint = respawn.position;
        }
    }
}
