using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsInfo;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToCredits()
    {
        mainMenu.SetActive(false);
        creditsInfo.SetActive(true);
    }

    public void BackToMenu()
    {
        creditsInfo.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
