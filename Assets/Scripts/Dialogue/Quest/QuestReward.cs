using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward 
{
    public int gold;
    public int experience;

    public void ClaimReward()
    {
        Inventory.gold += gold;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().GainExp(experience);
    }
}
