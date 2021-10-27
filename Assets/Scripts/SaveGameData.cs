using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MainScript;

public static class SaveGameData
{
    public static void SaveData(PlayFabManager data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/autofill.bin";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData gamedata = new GameData(data);

        formatter.Serialize(stream, gamedata);
        stream.Close();
    }
    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/autofill.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            Debug.Log("Required File loaded from " + path);
            return data;
        }
        else
        {
            Debug.LogError("Required File not found at " + path);
            return null;
        }
    }
    public static void SaveCollectablesData(CollectablesManager data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/collectables.bin";

        FileStream stream = new FileStream(path, FileMode.Create);

        CollectablesData collectablesData = new CollectablesData(data);

        formatter.Serialize(stream, collectablesData);
        stream.Close();
    }
    public static CollectablesData LoadCollectablesData()
    {
        string path = Application.persistentDataPath + "/collectables.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CollectablesData collectablesData = formatter.Deserialize(stream) as CollectablesData;
            stream.Close();
            Debug.Log("Required File loaded from " + path);
            return collectablesData;
        }
        else
        {
            Debug.LogError("Required File not found at " + path);
            return null;
        }
    }
}
