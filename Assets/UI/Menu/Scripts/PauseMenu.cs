using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject dieMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Character characterScript;
    [SerializeField] private Slider volumeSlider;

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
        characterScript.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
        characterScript.gameObject.SetActive(true);
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
        GameMaster.instance.lastCheckPoint = GameMaster.instance.levelStart;
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void ToMenu()
    {
        GameMaster.instance.lastCheckPoint = GameMaster.instance.levelStart;
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
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
