using System.Collections.Generic;
using UnityEngine;

public static class ZoneManager
{
    public static Dictionary<string, Zone> Zones { get; } = new Dictionary<string, Zone>();
    public static Dictionary<Vector3Int, Zone> ZoneNodes { get; } = new Dictionary<Vector3Int, Zone>();


    // TODO: add extending zones
    public static void MakeNewZone(HashSet<Vector3Int> nodes)
    {
        foreach (var node in nodes)
        {
            if (ZoneNodes.ContainsKey(node))
            {
                Debug.LogWarning("A zone already exists here!");
                return;
            }
        }

        Zone zone = new Zone(nodes);
    }

    // TODO: zones shouldn't be allowed to split in half when removing
    public static void RemoveZone(HashSet<Vector3Int> nodes)
    {
        foreach (var node in nodes)
        {
            if (ZoneNodes.TryGetValue(node, out Zone zone))
            {
                zone.RemoveNode(node);
            }
        }
    }

    public static bool ExistsZoneWithFreeSpace(Item item)
    {
        foreach (var zone in Zones.Values)
        {
            int spaceInZone = zone.GetTotalCapacityForItem(item);

            if (spaceInZone > 0)
            {
                return true;
            }
        }

        return false;
    }

    public static bool TryFindZoneWithUnreservedSpaceForItem(Item item, out Zone foundZone, out HaulDestination haulDestination, out int totalSpace)
    {
        foreach (var zone in Zones.Values)
        {
            int spaceInZone = zone.GetTotalCapacityForItem(item);

            if (spaceInZone > 0)
            {
                HaulDestination destination = zone.GetHaulDestination(item);
                if (destination.Type != HaulDestination.DestinationType.None)
                {
                    foundZone = zone;
                    haulDestination = destination;
                    totalSpace = spaceInZone;
                    return true;
                }
            }
        }

        foundZone = null;
        totalSpace = 0;
        haulDestination = null;
        return false;
    }

    public static void Reset()
    {
        Zones.Clear();
        ZoneNodes.Clear();
    }
}

public class ZoneManagerSaver { }