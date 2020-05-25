using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveAndLoad : MonoBehaviour
{
    public PlayerControl playerStats;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Loaded")) {
            FirstLoad();
            PlayerPrefs.SetInt("Loaded", 0);
            Save();
        }
        else
        {
            Load();
        }
    }


    void FirstLoad()
    {

        Debug.Log("New game");
        playerStats.transform.position = new Vector3(290, 3, 70);

    }

    public void Save()
    {
        PlayerBinary.SavePlayerData(playerStats);
    }

    public void Load()
    {
        PlayerData data = PlayerBinary.LoadPlayerData(playerStats);
        playerStats.name = data.playerName;
        playerStats.level = data.level;
       
        playerStats.transform.position = new Vector3(data.pX, data.pY, data.pZ);
    }

}
