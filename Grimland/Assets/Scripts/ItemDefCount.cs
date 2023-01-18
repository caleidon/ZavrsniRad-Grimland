using System.Collections.Generic;
using UnityEngine;

public class ItemDefCount
{
    public ItemDef ItemDef { get; set; }
    public int Count { get; set; }

    public ItemDefCount(ItemDef def, int count)
    {
        ItemDef = def;
        Count = count;
    }

    public static int CalculateSlotsForInventory(IEnumerable<ItemDefCount> thingDefCounts)
    {
        int slotsNeeded = 0;

        foreach (var thingDefCount in thingDefCounts)
        {
            int maxThingStackSize = thingDefCount.ItemDef.MaxStackSize;

            slotsNeeded += Mathf.CeilToInt((float) thingDefCount.Count / maxThingStackSize);
        }

        return slotsNeeded;
    }
}