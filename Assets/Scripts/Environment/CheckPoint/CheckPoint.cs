using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject player;

    private Vector3 playerPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        player.transform.position = playerPosition;
        player.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Character>().checkPoint = gameObject;
            playerPosition = other.gameObject.transform.position;

        }
    }
}
