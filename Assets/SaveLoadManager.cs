using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public int version = 1;
    public static string savePath;

    private void Start()
    {
        instance = this;
        savePath = Application.persistentDataPath + "/Characters/";
        Directory.CreateDirectory(Application.persistentDataPath + "/Characters");
    }

    public void SaveCharacter(Player player)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath + player.entityName + ".sav", FileMode.Create);

        PlayerSaveData data = new PlayerSaveData(player);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public PlayerSaveData LoadCharacter(string name)
    {
        if(File.Exists(savePath + name + ".sav"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(savePath + name + ".sav", FileMode.Open);

            PlayerSaveData data = bf.Deserialize(stream) as PlayerSaveData;
            

            stream.Close();

            return data;
        }

        return null;
    }

    public List<PlayerSaveData> FindCharacters()
    {
        List<PlayerSaveData> list = new List<PlayerSaveData>();

        foreach (string file in Directory.GetFiles(savePath))
        {
            Debug.Log("File found - " + file);
            FileStream stream = new FileStream(file, FileMode.Open);

            if (File.Exists(file) && stream.Length > 0)
            {

                BinaryFormatter bf = new BinaryFormatter();

                PlayerSaveData data = bf.Deserialize(stream) as PlayerSaveData;


                list.Add(data);

            }

            stream.Close();

        }

        return list;
    }

}

