using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(GameManager gameManager, PlayerStats playerStats, Timer timer)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetDataPath();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gameManager, playerStats, timer);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game saved at " + path);
    }


    public static SaveData LoadPlayer()
    {
        string path = GetDataPath();
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    public static void ResetData()
    {
        string path = GetDataPath();
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at " + path);
        }
    }

    private static string GetDataPath()
    {
        return Application.persistentDataPath + "/saveData.mega";
    }
}
