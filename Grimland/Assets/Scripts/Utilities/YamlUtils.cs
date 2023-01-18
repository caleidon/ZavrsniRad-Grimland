using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class YamlUtils
{
    public static List<ItemDefCount> ThingDefCountFromYaml(List<Dictionary<string, int>> requiredMaterials)
    {
        List<ItemDefCount> thingDefCounts = new List<ItemDefCount>();

        foreach (Dictionary<string, int> pair in requiredMaterials)
        {
            KeyValuePair<string, int> kvPair = pair.First();
            ItemDef def = (ItemDef) DefManager.Defs.Value<ThingDef>(kvPair.Key);
            int count = kvPair.Value;
            ItemDefCount itemDefCount = new ItemDefCount(def, count);
            thingDefCounts.Add(itemDefCount);
        }

        return thingDefCounts;
    }

    public static Color ColorFromYaml(string color)
    {
        string[] rgbValues = color.Split(',');
        return new Color(float.Parse(rgbValues[0]), float.Parse(rgbValues[1]), float.Parse(rgbValues[2]),
            float.Parse(rgbValues[3]));
    }

    //TODO define min and max genheight directly in yaml maybe?
    public static float GenHeightFromYaml(string genHeight)
    {
        string[] genHeightValues = genHeight.Split('-');
        float minValue = float.Parse(genHeightValues[0]);
        float maxValue = float.Parse(genHeightValues[1]);
        return Random.Range(minValue, maxValue);
    }

    public static int GetRandomIntFromRange(string min, string max)
    {
        return Random.Range(int.Parse(min), int.Parse(max) + 1);
    }

    public static float GetRandomFloatFromRange(string min, string max)
    {
        return Random.Range(float.Parse(min), float.Parse(max));
    }
}