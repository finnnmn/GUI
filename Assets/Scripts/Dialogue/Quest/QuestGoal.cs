using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public int ID;
    public string itemName;
    public int amount;
    public int requiredAmount;
    
    public bool CheckGoal()
    {
        amount = 0;
        foreach (Item item in Inventory.inventory)
        {
            if (item.ID == ID)
            {
                amount += item.Amount;
            }
        }
        if (amount >= requiredAmount)
        {
            return true;
        }
        return false;
    }

    public void GiveItems()
    {
        int remainingAmount = requiredAmount;
        for (int i = Inventory.inventory.Count - 1; i >= 0; i--)
        {
            if (Inventory.inventory[i].ID == ID)
            {
                if (Inventory.inventory[i].Amount > 1)
                {
                    while(Inventory.inventory[i].Amount > 1)
                    {
                        if (remainingAmount == 0)
                        {
                            break;
                        }
                        Inventory.RemoveItem(i, 1);
                        remainingAmount -= 1;
                    }
                }
                if (Inventory.inventory[i].Amount == 1 && remainingAmount > 0)
                {
                    Inventory.RemoveItem(i, 1);
                    remainingAmount -= 1;
                }
            }
            if (remainingAmount == 0)
            {
                break;
            }
        }
    }
}
