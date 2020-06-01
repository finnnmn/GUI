using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerBinary
{

    public static void SaveNewData(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SavePlayerData(PlayerControl player)
    {
        //Reference a binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        //location to save to
        string path = Application.persistentDataPath + "/" + "save";

        //create a file at save path
        FileStream stream = new FileStream(path, FileMode.Create);

        //what data to write to the file
        PlayerData data = new PlayerData(player);

        //write and convert to bytes for writing to binary
        formatter.Serialize(stream, data);

        stream.Close();
    }
    public static PlayerData LoadPlayerData()
    {
        //Location to Load from
        string path = Application.persistentDataPath + "/" + "save";

        //if that location exists
        if(File.Exists(path))
        {
            //Get binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //read the data from the path
            FileStream stream = new FileStream(path, FileMode.Open);

            //set the data from what it was to usable variables
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
