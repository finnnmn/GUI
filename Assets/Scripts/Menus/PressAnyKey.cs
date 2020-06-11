using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PressAnyKey : MonoBehaviour
{
    public GameObject menuPanel, pressAnyKeyPanel;
    public AudioSource menuMusic;
    public static bool started;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;

        if (!started) {
            menuPanel.SetActive(false);
            pressAnyKeyPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(true);
            pressAnyKeyPanel.SetActive(false);
            menuMusic.Play();
        }
    }

    void Update()
    {
        if (!started) { 
            if (Input.anyKeyDown)
            {
                GameObject.FindGameObjectWithTag("MenuHandler").GetComponent<MenuHandler>().ButtonSound();
                menuPanel.SetActive(true);
                pressAnyKeyPanel.SetActive(false);
                menuMusic.Play();
                started = true;
            }
        }
    }
}
