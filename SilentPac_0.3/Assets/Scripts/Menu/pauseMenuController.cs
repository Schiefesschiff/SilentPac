using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuMain;
    public GameObject pauseMenuOptions;
    public GameObject pauseMenuControls;

    //private GameObject hud;
    //private Canvas hudCanvas;

    private void Start()
    {
        //this.transform.FindChild("PauseMenuMain").gameObject;
        
        //GameObject pauseMenuMain = this.transform.GetChild(0).gameObject;
        //GameObject pauseMenuOptions = this.transform.GetChild(1).gameObject;

        //pauseMenuMain = GameObject.Find("PauseMenuMain");
        //pauseMenuOptions = GameObject.Find("PauseMenuOptions");

        //hud = GameObject.FindGameObjectWithTag("HUD");
        //hudCanvas = hud.GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //PauseMenu navigation.
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else if (pauseMenuMain.activeSelf) //main to game
            {
                Debug.Log("Resuming Game.");
                ResumeGame();
            }
            else if (pauseMenuOptions.activeSelf) //options to main
            {
                pauseMenuOptions.SetActive(false);
                pauseMenuMain.SetActive(true);
            }
            else if (pauseMenuControls.activeSelf) //controls to main
            {
                pauseMenuControls.SetActive(false);
                pauseMenuMain.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Unpausing Game.");
        pauseMenuMain.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void PauseGame()
    {
        Debug.Log("Pausing Game.");
        //hudCanvas.SetActive(false);
        pauseMenuMain.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void RestartLevel()
    {
        ResumeGame();
        Debug.Log("Restarting current level.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
