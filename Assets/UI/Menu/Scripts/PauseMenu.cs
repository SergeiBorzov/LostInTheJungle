using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject dieMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Character characterScript;
    private bool gamePaused = false;

    private void Pause()
    {
        pauseMenu.SetActive(true);
        characterScript.isGui = true;
        characterScript.GetComponents<AudioSource>()[4].Stop();
        Time.timeScale = 0.0f;
        gamePaused = true;
    }


    public void LastCheckPoint()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        characterScript.isGui = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gamePaused = false;
        characterScript.GetComponents<AudioSource>()[4].Play();
    }

    public void Restart()
    {
        GameMaster.lastCheckPoint = GameMaster.levelStart;
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void ToMenu()
    {
        GameMaster.lastCheckPoint = GameMaster.levelStart;
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !characterScript.isDead)
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (characterScript.isDead)
        {
            dieMenu.SetActive(true);
        }
    }
}
