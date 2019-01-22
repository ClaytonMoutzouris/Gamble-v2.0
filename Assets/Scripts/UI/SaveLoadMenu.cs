using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using System.IO;

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
    public int version = 2;

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
        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        //The "using" syntax defines a scope for which the writer is valid, automatically closing it when exiting the scope
        //This is because BinaryWriter implements the IDisposable interface, which have Dispose methods. Exiting our using scope will invoke this method
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            //This is our header byte, which will let us know which version of the save we are using
            //right now it is byte 0, so version 0. if we update our format we can use this byte
            //to determine what format a map we are loading is
            writer.Write(version);

            mMap.chunk.Save(writer);

            Debug.Log("Saving EdgeType :" + mMap.chunk.edgeType);
            Debug.Log("Saving ChunkType :" + mMap.chunk.type);
        }

    }

    public void Load(string path)
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
                mMap.chunk.Load(reader);
                mMap.Draw();
                Debug.Log("Loading EdgeType :" + mMap.chunk.edgeType);
                Debug.Log("Loading ChunkType :" + mMap.chunk.type);

                RoomEditor.SetEditorValues(mMap.chunk.type, mMap.chunk.edgeType);
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
        if (selected != null)
        {
            path = selected.Path;

        } else
        {
            path = Application.dataPath + "/Rooms/" + nameInput.text + ".room";
        }

        if (path == null)
        {
            return;
        }
        if (saveMode)
        {
            Save(path);
        }
        else
        {
            Load(path);
        }
        Close();
    }

    public void SelectItem(SaveLoadItem selected)
    {
        nameInput.text = selected.MapName;
        this.selected = selected;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }

        string[] paths =
            Directory.GetFiles(Application.dataPath + "/Rooms", "*.room", SearchOption.AllDirectories);

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
        string path = selected.Path;
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
}