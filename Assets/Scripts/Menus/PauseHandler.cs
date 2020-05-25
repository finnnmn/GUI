using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public static bool paused;
    public GameObject pausePanel, optionsPanel;
    void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                if (optionsPanel.activeSelf)
                {
                    optionsPanel.SetActive(false);
                    pausePanel.SetActive(true);
                    GetComponent<MenuHandler>().SavePlayerPrefs();
                    GetComponent<KeybindManager>().SaveKeys();

                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        //set paused to true and open the menu
        paused = true;
        pausePanel.SetActive(true);
        //make the cursor visible and allow it to move
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //set the timescale to 0 so the game pauses
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //set paused to false and hide the pause menu
        paused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        //hide the cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //set the timescale to 1 so the game runs at normal speed
        Time.timeScale = 1;
    }
}
