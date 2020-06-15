using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public PlayerControl player;
    [Header("UI References")]
    public GameObject characterPanel;
    public Text nameText;
    public Text infoText;
    public Text healthText;
    public Text manaText;
    public Text staminaText;
    public Text strengthText;
    public Text dexterityText;
    public Text constitutionText;
    public Text intelligenceText;
    public Text wisdomText;
    public Text charismaText;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        CloseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(player.openKey) && (player.currentMenu == null || player.currentMenu == "Character"))
        {
            GameObject.FindGameObjectWithTag("MenuHandler").GetComponent<MenuHandler>().ButtonSound();
            if (!PauseHandler.paused)
            {
                OpenMenu();
            }
            else
            {
                GetComponent<CharacterMenu>().CloseMenu();
            }
        }
    }
    public void OpenMenu()
    {
        SetValues();
        characterPanel.SetActive(true);
        PauseHandler.Pause();
        PauseHandler.paused = true;
        player.currentMenu = "Character";
    }

    public void CloseMenu()
    {
        characterPanel.SetActive(false);
        PauseHandler.Resume();
        PauseHandler.paused = false;
        player.currentMenu = null;
    }

    void SetValues()
    {
        nameText.text = player.playerName;
        infoText.text = "Level " + player.level + " " + player.playerClass + "\n" + "Exp " + player.exp + "/" + player.maxExp;

        healthText.text = "Health: " + Math.Floor(player.characterStatus[0].currentValue) + "/" + player.characterStatus[0].maxValue;
        manaText.text = "Mana: " + Math.Floor(player.characterStatus[1].currentValue) + "/" + player.characterStatus[1].maxValue;
        staminaText.text = "Stamina: " + Math.Floor(player.characterStatus[2].currentValue) + "/" + player.characterStatus[2].maxValue;

        strengthText.text = "Str:" + player.characterStats[0].value;
        dexterityText.text = "Dex:" + player.characterStats[1].value;
        constitutionText.text = "Con:" + player.characterStats[2].value;
        intelligenceText.text = "Int:" + player.characterStats[3].value;
        wisdomText.text = "Wis:" + player.characterStats[4].value;
        charismaText.text = "Cha:" + player.characterStats[5].value;
    }


}
