using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class WorldNameGenerator
{
    public static Dictionary<WorldType, List<string>> nameParts;
    static bool initialized = false;

    // Start is called before the first frame update
    public static void Initialize()
    {
        initialized = true;

        nameParts = new Dictionary<WorldType, List<string>>();

        List<string> forestParts = new List<string>();
        forestParts.Add("brush");
        forestParts.Add("tera");
        forestParts.Add("geo");
        forestParts.Add("tel");
        forestParts.Add("apis");
        forestParts.Add("ferous");
        forestParts.Add("con");

        nameParts.Add(WorldType.Forest, forestParts);

        List<string> iceParts = new List<string>();
        iceParts.Add("crys");
        iceParts.Add("burr");
        iceParts.Add("pol");
        iceParts.Add("tund");
        iceParts.Add("arc");
        iceParts.Add("bar");
        iceParts.Add("stil");

        nameParts.Add(WorldType.Tundra, iceParts);

        List<string> lavaParts = new List<string>();
        lavaParts.Add("coal");
        lavaParts.Add("ash");
        lavaParts.Add("tar");
        lavaParts.Add("bur");
        lavaParts.Add("inci");
        lavaParts.Add("trem");
        lavaParts.Add("ony");

        nameParts.Add(WorldType.Lava, lavaParts);

        List<string> purpleParts = new List<string>();
        purpleParts.Add("tris");
        purpleParts.Add("oysn");
        purpleParts.Add("gel");
        purpleParts.Add("vel");
        purpleParts.Add("onar");
        purpleParts.Add("quis");
        purpleParts.Add("ago");

        nameParts.Add(WorldType.Purple, purpleParts);

        List<string> yellowParts = new List<string>();
        yellowParts.Add("shroo");
        yellowParts.Add("puff");
        yellowParts.Add("moon");
        yellowParts.Add("fung");
        yellowParts.Add("spor");
        yellowParts.Add("ortis");
        yellowParts.Add("oni");

        nameParts.Add(WorldType.Yellow, yellowParts);

        List<string> voidParts = new List<string>();
        voidParts.Add("ecktar");
        voidParts.Add("umris");
        voidParts.Add("vok");
        voidParts.Add("star");
        voidParts.Add("drakk");
        voidParts.Add("hol");
        voidParts.Add("eclip");

        nameParts.Add(WorldType.Void, voidParts);


    }

    public static string GetNameForWorld(WorldType worldType)
    {
        if(!initialized)
        {
            Initialize();
        }

        string name = "";

        int rand1 = Random.Range(0, nameParts[worldType].Count);
        int rand2 = Random.Range(0, nameParts[worldType].Count);

        name += nameParts[worldType][rand1] + nameParts[worldType][rand2];

        return UppercaseFirst(name);
    }

    static string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

}
