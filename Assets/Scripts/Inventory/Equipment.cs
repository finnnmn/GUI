using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region slot struct
[System.Serializable]
public struct EquipmentSlot
{
    public string name;
    public Item item;
    public Button button;
    [Header("Show on character")]
    public bool show;
    public Transform location;
    public GameObject mesh;


}
#endregion

[RequireComponent(typeof(Inventory))]
public class Equipment : MonoBehaviour
{
    Inventory inventory;
    PlayerControl player;

    [Header("Equipment slots")]
    public EquipmentSlot[] equipmentSlots = new EquipmentSlot[4];

    [Header("UI references")]
    public Text nameText;
    public Text attackText;
    public Text defenceText;

    #region start
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        player = GetComponent<PlayerControl>();
        UpdateButtons();
        UpdateText();
    }
    #endregion

    #region equip
    public void EquipItem(Item item, EquipmentType equipmentType)
    {
        Unequip(equipmentType);
        equipmentSlots[(int)equipmentType].item = item;
        if (equipmentSlots[(int)equipmentType].show)
        {
            //spawn item in world on player
            equipmentSlots[(int)equipmentType].mesh = Instantiate(item.Mesh, equipmentSlots[(int)equipmentType].location);
            //set object layer to 9 (player layer) so camera renders it
            equipmentSlots[(int)equipmentType].mesh.layer = 9;
            //stop item from being collectable when equipped
            if (equipmentSlots[(int)equipmentType].mesh.GetComponent<ItemHandler>())
            {
                equipmentSlots[(int)equipmentType].mesh.GetComponent<ItemHandler>().collectable = false;
            }
        }
        //set item equipped value
        item.Equipped = true;

        //change player attack/defence
        if (item.Type == ItemType.Weapon)
        {
            player.attack += item.Power;
        }
        else
        {
            player.defence += item.Power;
        }

        //update buttons and text
        UpdateButton((int)equipmentType);
        UpdateText();
    }
    #endregion

    #region unequip
    public void Unequip(EquipmentType equipmentType)
    {
        if (equipmentSlots[(int)equipmentType].item != null)
        {
            //change player attack/defence
            if (equipmentSlots[(int)equipmentType].item.Type == ItemType.Weapon)
            {
                player.attack -= equipmentSlots[(int)equipmentType].item.Power;
            }
            else
            {
                player.defence -= equipmentSlots[(int)equipmentType].item.Power;
            }

            //destroy object in world
            if (equipmentSlots[(int)equipmentType].show)
            {
                GameObject.Destroy(equipmentSlots[(int)equipmentType].mesh);
            }
            //make item not equipped
            equipmentSlots[(int)equipmentType].item.Equipped = false;
            //add item to inventory
            Inventory.AddItem((equipmentSlots[(int)equipmentType].item));
            //remove from equipment
            equipmentSlots[(int)equipmentType].item = null;
        }

        //update buttons and text
        UpdateButton((int)equipmentType);
        UpdateText();
    }

    #endregion

    #region buttons
    void UpdateButton(int number)
    {
        if (equipmentSlots[number].item == null)
        {
            equipmentSlots[number].button.interactable = false;
            equipmentSlots[number].button.GetComponentInChildren<Text>().text = "Empty";
        }
        else
        {
            equipmentSlots[number].button.interactable = true;
            equipmentSlots[number].button.GetComponentInChildren<Text>().text = equipmentSlots[number].item.Name;
            equipmentSlots[number].button.onClick.RemoveAllListeners();
            equipmentSlots[number].button.onClick.AddListener(() => inventory.SelectItem(equipmentSlots[number].item));
        }
    }

    void UpdateButtons()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            UpdateButton(i);
        }
    }
    #endregion

    #region text
    void UpdateText()
    {
        nameText.text = player.playerName;
        attackText.text = "ATK: " + player.attack;
        defenceText.text = "DEF: " + player.defence;
    }
    #endregion
}

public enum EquipmentType
{
    hand1,
    hand2,
    head,
    body
}
