using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerSaveAndLoad : MonoBehaviour
{
    public PlayerControl playerStats;

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "save"))
        {
            Load();
        }
        else
        {
            Debug.LogError("Save file did not exist");
           
            SceneManager.LoadScene(0);
        }
    }

    public void Save()
    {
        PlayerBinary.SavePlayerData(playerStats);
    }

    public void Load()
    {
        PlayerData data = PlayerBinary.LoadPlayerData();
        playerStats.playerName = data.playerName;
        playerStats.gameObject.name = data.playerName;
        playerStats.playerClass = data.playerClass;
        playerStats.level = data.level;

        for (int i = 0; i < playerStats.characterStatus.Length; i++)
        {
            playerStats.characterStatus[i].currentValue = data.lifeValue[i];
        }

        for (int i = 0; i < playerStats.characterStats.Length; i++)
        {
            playerStats.characterStats[i].value = data.characterStats[i];
        }

        playerStats.transform.position = new Vector3(data.pX, data.pY, data.pZ);

        playerStats.customIndex = data.customIndex;

        playerStats.SetCustomisation();
    }

}
