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
                menuPanel.SetActive(true);
                pressAnyKeyPanel.SetActive(false);
                menuMusic.Play();
                started = true;
            }
        }
    }
}
