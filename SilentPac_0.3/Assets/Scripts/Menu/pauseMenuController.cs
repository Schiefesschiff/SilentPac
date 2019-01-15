using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    //private GameObject hud;
    //private Canvas hudCanvas;

    private void Awake()
    {
        //hud = GameObject.FindGameObjectWithTag("HUD");
        //hudCanvas = hud.GetComponent<Canvas>();
    }

    private void Update()
    {   // || Input.GetKeyDown(StringCollection.INPUT_START)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused && pauseMenuUI.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Unpausing Game.");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void PauseGame()
    {
        Debug.Log("Pausing Game.");
        //hudCanvas.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void RestartLevel()
    {
        ResumeGame();
        Debug.Log("Restarting level.");
        SceneManager.LoadScene(1);
    }

    public void ExitToMenu()
    {
        ResumeGame();
        Debug.Log("Loading MenuScene.");
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        ResumeGame();
        Debug.Log("Quitting Game.");
        Application.Quit();
    }
}
