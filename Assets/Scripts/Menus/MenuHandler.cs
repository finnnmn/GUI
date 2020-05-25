using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuHandler : MonoBehaviour
{

    #region options variables
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    [Header("UI references")]
    public Slider volumeSlider;
    public Slider soundSlider;
    public Toggle muteToggle;
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    #endregion

    private void Start()
    {
        //set the options to the playerprefs
        #region volume and mute
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0);
            audioMixer.SetFloat("musicVolume", 0);
            PlayerPrefs.Save();
        }
        else
        {
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));
        }

        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 0);
            audioMixer.SetFloat("soundVolume", 0);
            PlayerPrefs.Save();
        }
        else
        {
            audioMixer.SetFloat("soundVolume", PlayerPrefs.GetFloat("soundVolume"));
        }

        if (!PlayerPrefs.HasKey("mute"))
        {
            PlayerPrefs.SetInt("mute", 0);
            audioMixer.SetFloat("master", 0);
            PlayerPrefs.Save();
        }
        else
        {
            if (PlayerPrefs.GetInt("mute") == 0)
            {
                audioMixer.SetFloat("master", 0);
            }
            else
            {
                audioMixer.SetFloat("master", -80);
            }
        }

        #endregion

        #region quality and fullscreen
        if (!PlayerPrefs.HasKey("quality"))
        {
            PlayerPrefs.SetInt("quality", 1);
            QualitySettings.SetQualityLevel(1);
            PlayerPrefs.Save();
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }

        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 0);
            Screen.fullScreen = false;
            PlayerPrefs.Save();
        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
        #endregion

        #region resolution
        //set up the resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endregion
    }
    public void ChangeScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }
    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void SetOptions()
    {
        //use playerprefs to set up the options in the menu as it is opened
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("soundVolume");

        if (PlayerPrefs.GetInt("mute") == 0)
        {
            muteToggle.isOn = false;
        }
        else
        {
            muteToggle.isOn = true;
        }


        qualityDropdown.value = PlayerPrefs.GetInt("quality");
        if (PlayerPrefs.GetInt("fullscreen") == 0)
        {
            fullscreenToggle.isOn = false;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }
    }

    public void SetDefaults()
    {
        //Set all options and gui elements to their default values
        audioMixer.SetFloat("musicVolume", 0f);
        volumeSlider.value = 0;

        audioMixer.SetFloat("soundVolume", 0f);
        soundSlider.value = 0;

        audioMixer.SetFloat("master", 0);
        muteToggle.isOn = false;

        QualitySettings.SetQualityLevel(1);
        qualityDropdown.value = QualitySettings.GetQualityLevel();


    }



    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);  
    }

    public void ChangeSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
    }

    public void MuteVolume(bool muted)
    {
        if (muted)
        {
            audioMixer.SetFloat("master", -80);
        }
        else
        {
            audioMixer.SetFloat("master", 0);
        }
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void ChangeResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SavePlayerPrefs()
    {
        //when closing options save everything to playerprefs
        float value;
        audioMixer.GetFloat("musicVolume", out value);
        PlayerPrefs.SetFloat("musicVolume", value);
        audioMixer.GetFloat("soundVolume", out value);
        PlayerPrefs.SetFloat("soundVolume", value);

        audioMixer.GetFloat("master", out value);
        if (value == 0)
        {
            PlayerPrefs.SetInt("mute", 0);
        }
        else
        {
            PlayerPrefs.SetInt("mute", 1);
        }

        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());

        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        PlayerPrefs.Save();


    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("Loaded");
    }
}
