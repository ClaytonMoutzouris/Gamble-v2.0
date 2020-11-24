using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadMenu : MonoBehaviour
{
    public Text menuLabel, actionButtonLabel;
    bool saveMode;
    public EditorMap mMap;
    public InputField nameInput;
    public SaveLoadItem selected;
    public RectTransform listContent;
    public SaveLoadItem itemPrefab;
    public RoomEditor RoomEditor;
    public int version = 3;

    public void Open(bool saveMode)
    {
        FillList();
        if (saveMode)
        {
            menuLabel.text = "Save Room";
            actionButtonLabel.text = "Save";
        }
        else
        {
            menuLabel.text = "Load Room";
            actionButtonLabel.text = "Load";
        }

        this.saveMode = saveMode;
        gameObject.SetActive(true);
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Save(string path)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        bf.Serialize(stream, mMap.room);
        stream.Close();
    }

    public void Load(string path)
    {
        if (File.Exists(path))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            RoomData data = bf.Deserialize(stream) as RoomData;
            //Sdata.entityData = new EntityData[data.mWidth, data.mHeight];

            stream.Close();

            mMap.room = data;
            
            mMap.Draw();
            RoomEditor.UpdateEditorValues();

        }

    }

    public void OldSave(string path)
    {
        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        //The "using" syntax defines a scope for which the writer is valid, automatically closing it when exiting the scope
        //This is because BinaryWriter implements the IDisposable interface, which have Dispose methods. Exiting our using scope will invoke this method
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            //This is our header byte, which will let us know which version of the save we are using
            //right now it is byte 0, so version 0. if we update our format we can use this byte
            //to determine what format a map we are loading is
            writer.Write(version);

            Debug.Log("Room Size: " + mMap.room.mWidth + ", " + mMap.room.mHeight);
            mMap.room.Save(writer);

        }

    }

    public void OldLoad(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }

        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == version)
            {
                mMap.room.Load(reader);
                mMap.Draw();
                Debug.Log("Room Size: " + mMap.room.mWidth + ", " + mMap.room.mHeight);
                Debug.Log("Room Type: " + mMap.room.roomType + ", World Type: " + mMap.room.worldType);

                RoomEditor.UpdateEditorValues();
            }
            else
            {
                Debug.LogWarning("Unknown room format " + header);
            }

        }
    }

    public void Action()
    {
        string path;



        if (saveMode)
        {
            path = Application.dataPath + "/RoomsWorkingDir2/" + nameInput.text + ".txt";


            if (path == null)
            {
                Directory.CreateDirectory(Application.dataPath + "/RoomsWorkingDir2");
            }
            Save(path);
        }
        else
        {
            path = Application.dataPath + "/RoomsWorkingDir2/" + nameInput.text + ".txt";


            if (path == null)
            {
                Directory.CreateDirectory(Application.dataPath + "/RoomsWorkingDir2");
            }
            Load(path);
        }
        Close();
    }

    public void SelectItem(SaveLoadItem selected)
    {
        nameInput.text = selected.MapName;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }

        string[] paths =
            Directory.GetFiles(Application.dataPath + "/RoomsWorkingDir2", "*.txt", SearchOption.AllDirectories);

        Array.Sort(paths);

        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
            item.menu = this;
            item.Path = paths[i];
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
            item.transform.SetParent(listContent, false);

        }
    }

    public void Delete()
    {
        string path;
        path = Application.dataPath + "/RoomsWorkingDir2/" + nameInput.text + ".txt";
        if (path == null)
        {
            return;
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        nameInput.text = "";
        FillList();
    }

    public void ConvertSaves()
    {

    }
}