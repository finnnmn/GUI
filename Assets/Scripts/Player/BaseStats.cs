using UnityEngine;
using UnityEngine.UI;

public class BaseStats : MonoBehaviour
{
    //stats inherited by player
    public string playerName;
    public int level;

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

    public LifeForceStatus[] characterStatus = new LifeForceStatus[3];

    //Stats
    [System.Serializable]
    public struct StatBlock
    {
        public string name;
        public int value;
        public int tempValue;
    }

    public StatBlock[] characterStats = new StatBlock[6];

    
}
