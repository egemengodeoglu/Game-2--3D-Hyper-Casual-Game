using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveBinary
{
    public static void SaveGameData(GameDataReferences playerData)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }
        string jsonData = JsonUtility.ToJson(playerData);

        string path= Application.persistentDataPath + "/Saves/GameData.dat";
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            binaryFormatter.Serialize(fileStream, jsonData);
        }
        //fileStream.Close();
    }

    public static void LoadGameData(GameDataReferences playerData)
    {
        string path = Application.persistentDataPath + "/Saves/GameData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string stringData = "";
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                stringData = binaryFormatter.Deserialize(fileStream) as string;

            }

            //fileStream.Close();
            JsonUtility.FromJsonOverwrite(stringData, playerData);
        }
    }
}
