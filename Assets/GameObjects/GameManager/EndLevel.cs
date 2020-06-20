using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Character>() != null)
        {
            GameMaster.instance.lastCheckPoint = GameMaster.instance.levelStart;
            SceneManager.LoadScene(0);
        }
    }
}
