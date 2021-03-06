﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindManager : MonoBehaviour
{
    private readonly Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public GameObject currentKey;
    public Color32 defaultColour = new Color32(96, 245, 229, 255);
    public Color32 selected = new Color32(32, 250, 43, 255);

    public GameObject player;
    [Header("Text elements")]

    public Text forward;
    public Text left, right, backward, jump, sprint, crouch, interact, inventory, questLog, character;

    private void Start()
    {
        //When adding keys, add to the next 2 statements, the variables above, in playerControl and on options panels

        //add the keys to the dictionary with default values
        keys.Add("Forward", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Backward", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S")));
        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
        keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift")));
        keys.Add("Crouch", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl")));
        keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E")));
        keys.Add("Inventory", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory", "Tab")));
        keys.Add("QuestLog", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("QuestLog", "L")));
        keys.Add("Character", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Character", "C")));

        //set the text on the buttons to the keycodes
        forward.text = keys["Forward"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        backward.text = keys["Backward"].ToString();
        jump.text = keys["Jump"].ToString();
        sprint.text = keys["Sprint"].ToString();
        crouch.text = keys["Crouch"].ToString();
        interact.text = keys["Interact"].ToString();
        inventory.text = keys["Inventory"].ToString();
        questLog.text = keys["QuestLog"].ToString();
        character.text = keys["Character"].ToString();

    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                //set keycode to pressed key on the selected button
                keys[currentKey.name] = e.keyCode;
                currentKey.GetComponentInChildren<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = defaultColour;
                currentKey = null;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                keys[currentKey.name] = (KeyCode)(Enum.Parse(typeof(KeyCode), "LeftShift"));
                currentKey.GetComponentInChildren<Text>().text = "LeftShift";
                currentKey.GetComponent<Image>().color = defaultColour;
                currentKey = null;
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                keys[currentKey.name] = (KeyCode)(Enum.Parse(typeof(KeyCode), "RightShift"));
                currentKey.GetComponentInChildren<Text>().text = "RightShift";
                currentKey.GetComponent<Image>().color = defaultColour;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        //set colours of buttons when selected
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = defaultColour;
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        //save new keys to playerprefs
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();

        if (player != null)
        {
            player.GetComponent<PlayerControl>().GetKeyBindings();
        }
    }

}
