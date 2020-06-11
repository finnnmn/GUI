using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [Header("Type")]
    public StorageType storageType;
    public string inventoryName;
    public List<Item> inventory = new List<Item>();
    [Header("Default items")]
    public DefaultItem[] defaultItems = new DefaultItem[0];
    [HideInInspector]
    public int storage;
    [Header("Chest values")]
    public int maxStorage = 20;
    [Header("Shop values")]
    public float buyMultiplier = 1.2f;
    public float sellMultiplier = 0.5f;


    Inventory inventoryScript;

    #region start
    private void Start()
    {
        inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //default inventory
        if (defaultItems.Length > 0)
        {
            for (int i = 0; i < defaultItems.Length; i++)
            {
                if (defaultItems[i].amount > 0)
                {
                    AddItem(ItemData.CreateItem(defaultItems[i].ID, defaultItems[i].amount));
                }
            }
        }
    }

    #endregion

    #region open
    public void Open()
    {
        switch(storageType)
        {
            case StorageType.chest:
                inventoryScript.inventoryMode = InventoryMode.chest;
                inventoryScript.currentStorage = this;
                inventoryScript.OpenInventory();
                inventoryScript.DeselectStorage();
                break;
            case StorageType.shop:
                inventoryScript.inventoryMode = InventoryMode.shop;
                inventoryScript.currentStorage = this;
                inventoryScript.OpenInventory();
                inventoryScript.DeselectStorage();
                break;
        }
    }
    #endregion

    #region add item
    public void AddItem(Item item)
    {

       
        //check for stacking
        if (item.StackSize > 1)
        {
            int remainingAmount = item.Amount;
            do
            {
                //add to any existing stacks
                foreach (Item checkItem in inventory)
                {
                    if (checkItem.ID == item.ID)
                    {
                        while (checkItem.Amount < checkItem.StackSize && remainingAmount > 0)
                        {
                            checkItem.Amount += 1;
                            remainingAmount -= 1;
                        }
                    }
                }
                if (remainingAmount > 0)
                {
                    //make a new stack of 1 in inventory
                    AddNewStack(ItemData.CreateItem(item.ID, 1));
                    remainingAmount -= 1;

                }
            } while (remainingAmount > 0);
        }
        else
        {
            //add item as new
            AddNewStack(item);
        }
    }

    void AddNewStack(Item item)
    {
        storage += 1;
        inventory.Add(item);
    }
    #endregion

    #region remove item
    public void RemoveItem(int number, int amount)
    {
        inventory[number].Amount -= amount;
        if (inventory[number].Amount <= 0)
        {
            inventory.Remove(inventory[number]);
            storage -= 1;

            if (inventory.Count > number)
            {
                inventoryScript.SelectItem(inventory[number]);
            }
            else if (inventory.Count > 0)
            {
                inventoryScript.SelectItem(inventory[number - 1]);
            }
            else
            {
                inventoryScript.DeselectStorage();
            }
        }
        else
        {
            inventoryScript.SelectItem(inventory[number]);
        }

    }
    #endregion
}
[System.Serializable]
public struct DefaultItem
{
    public int ID;
    public int amount;
}
public enum StorageType
{
    chest,
    shop
}
