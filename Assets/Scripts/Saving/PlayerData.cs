[System.Serializable]
public class PlayerData
{
    //variables for saving
    public string playerName;
    public string playerClass;
    public int level;
    public float pX, pY, pZ;

    //health, mana, stamina
    public float[] lifeValue = new float[3];

    public int[] characterStats = new int[6];

    public int[] customIndex = new int[6];

    public PlayerData(PlayerControl player)
    {
        playerName = player.playerName;
        playerClass = player.playerClass;
        level = player.level;

        pX = player.transform.position.x;
        pY = player.transform.position.y;
        pZ = player.transform.position.z;

        for (int i = 0; i < player.characterStatus.Length; i++)
        {
            lifeValue[i] = player.characterStatus[i].currentValue;
        }

        for (int i = 0; i < characterStats.Length; i++)
        {
            characterStats[i] = player.characterStats[i].value;
        }
        
        customIndex = player.customIndex;
    }

    public PlayerData()
    {

    }



}
