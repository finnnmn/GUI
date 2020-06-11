using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public PlayerControl player;
    public static bool paused;
    public GameObject pausePanel, optionsPanel;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (player.currentMenu == null || player.currentMenu == "Pause")
            {
                GameObject.FindGameObjectWithTag("MenuHandler").GetComponent<MenuHandler>().ButtonSound();
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
            else if (player.currentMenu == "Inventory")
            {
                if (DialogueControl.inShop)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueControl>().CloseShop();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().CloseInventory();
                }
            }
            
        }
    }

    public static void Pause()
    {
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Pause();
        player.currentMenu = "Pause";
    }

    public void ResumeGame()
    {
        
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Resume();
        player.currentMenu = null;
    }

   
}
