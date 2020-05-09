using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public World hubWorld;
    public WorldData hubData;
    public List<World> worlds;
    public List<WorldData> worldDatas;
    public static WorldManager instance;
    public World currentWorld;
    public int currentWorldIndex = 0;

    private void Start()
    {
        instance = this;
        worlds = new List<World>();


        //worlds.Add(new World(worldDatas[0]));
        hubWorld = new World(hubData);
    }

    public void AddWorld(WorldType type)
    {
        World newWorld = new World(worldDatas[(int)type]);
        worlds.Add(newWorld);
        NavigationMenu.instance.AddWorldNode(newWorld);

    }

    public void SelectWorld(int index)
    {
        currentWorldIndex = index;


        GameManager.instance.mMapChangeFlag = true;

    }

    public void NewUniverse()
    {
        NavigationMenu.instance.ClearMaps();
        worlds.Clear();
        currentWorldIndex = -1;
    }

    public World GetHubWorld()
    {
        return hubWorld;
    }

    public void WorldCleared(int index)
    {
        worlds[index].worldCleared = true;
        NavigationMenu.instance.worldNodes[index].SetCleared();
    }

    public World GetCurrentWorld()
    {
        return worlds[currentWorldIndex];
    }


    public int NumCompletedWorlds()
    {
        int completed = 0;

        foreach(World world in worlds)
        {
            if(world.worldCleared)
            {
                completed++;
            }
        }
        return completed;

    }

}
