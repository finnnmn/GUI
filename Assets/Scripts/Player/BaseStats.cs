using UnityEngine;
using UnityEngine.UI;

//Health, mana, stamina
[System.Serializable]
public struct LifeForceStatus
{
    public string name;
    public float currentValue;
    public float maxValue;
    public float regenValue;
    public Image displayImage;
}

//Stats
[System.Serializable]
public struct StatBlock
{
    public string name;
    public int value;
    public int tempValue;
}

public class BaseStats : MonoBehaviour
{
    //stats inherited by player
    public string playerName;
    public string playerClass;
    public int level;
    public int exp;
    public int maxExp = 10;

    public LifeForceStatus[] characterStatus = new LifeForceStatus[3];

    public StatBlock[] characterStats = new StatBlock[6];

    public int[] customIndex = new int[6];

    public int attack;
    public int defence;

    public void GainExp(int amount)
    {
        exp += amount;
        if (exp >= maxExp)
        {
            int remainingExp = maxExp - exp;
            LevelUp();
            GainExp(remainingExp);
        }
    }

    public void LevelUp()
    {
        level += 1;
        exp = 0;
        maxExp += 5;

        for(int i = 0; i < characterStats.Length; i++)
        {
            characterStats[i].value += 1;
        }

        for (int i = 0; i < characterStatus.Length; i++)
        {
            characterStatus[i].maxValue += 5;
            characterStatus[i].currentValue += 5;
        }
    }
    
}
