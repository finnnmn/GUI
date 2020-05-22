[System.Serializable]
public class PlayerData
{
    //variables for saving
    public string playerName;
    public int level;
    public float pX, pY, pZ;

    public PlayerData(PlayerControl player)
    {
        playerName = player.name;
        level = player.level;

        pX = player.transform.position.x;
        pY = player.transform.position.y;
        pZ = player.transform.position.z;

    }



}
