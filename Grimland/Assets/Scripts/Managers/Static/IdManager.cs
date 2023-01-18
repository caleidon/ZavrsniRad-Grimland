using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding.Util;

public static class IdManager
{
    private static Dictionary<string, Thing> ThingList { get; } = new Dictionary<string, Thing>();

    public static uint NextThingID { get; set; }
    public static uint NextCreatureID { get; set; }
    public static uint NextJobID { get; set; }
    public static uint NextZoneID { get; set; }

    public static Thing GetThingById(string id)
    {
        return ThingList[id];
    }

    public static Dictionary<string, Thing>.ValueCollection GetAllThings()
    {
        return ThingList.Values;
    }

    public static string GenerateThingID(Thing thing, [CanBeNull] string existingId)
    {
        if (existingId != null)
        {
            ThingList.Add(existingId, thing);
            return existingId;
        }

        var id = $"{thing.ThingDef.Name}{NextThingID++}";
        ThingList.Add(id, thing);
        return id;
    }

    // public static string GenerateJobID(string jobName)
    // {
    //     if (existingId != null)
    //     {
    //         ThingList.Add(existingId, thing);
    //         return existingId;
    //     }
    //
    //     var id = $"{thing.ThingDef.Name}{NextThingID++}";
    //     ThingList.Add(id, thing);
    //     return id;
    //     
    //     return $"Job{NextJobID++}";
    // }

    public static string GenerateZoneID()
    {
        return $"Zone{NextZoneID++}";
    }

    public static string GenerateGUID()
    {
        return Guid.NewGuid().ToString();
    }

    public static void RemoveFromThingList(string id)
    {
        if (ThingList.ContainsKey(id))
        {
            ThingList.Remove(id);
        }
    }

    public static void Reset()
    {
        ThingList.Clear();
        NextThingID = 0;
        NextCreatureID = 0;
        NextJobID = 0;
        NextZoneID = 0;
    }
}

public class IdManagerSaver
{
    public uint NextThingID { get; set; }
    public uint NextCreatureID { get; set; }
    public uint NextJobID { get; set; }
    public uint NextZoneID { get; set; }

    public IdManagerSaver()
    {
        NextThingID = IdManager.NextThingID;
        NextCreatureID = IdManager.NextCreatureID;
        NextJobID = IdManager.NextJobID;
        NextZoneID = IdManager.NextZoneID;
    }

    public void Load()
    {
        IdManager.NextThingID = NextThingID;
        IdManager.NextCreatureID = NextCreatureID;
        IdManager.NextJobID = NextJobID;
        IdManager.NextZoneID = NextZoneID;
    }
}