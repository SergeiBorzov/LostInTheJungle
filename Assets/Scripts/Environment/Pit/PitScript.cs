using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PitScript : MonoBehaviour
{
    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Character playerScript = other.gameObject.GetComponent<Character>();
            if (playerScript.checkPoint != null)
            {
                playerScript.characterController.enabled = false;
                playerScript.checkPoint.GetComponent<CheckPoint>().RespawnPlayer();
                playerScript.characterController.enabled = true;
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
