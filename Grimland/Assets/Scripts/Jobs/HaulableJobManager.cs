using System;
using UnityEngine;

public static class HaulableJobManager
{
    public static void Notify_Spawned(Item item)
    {
        Check(item);
    }

    public static void Notify_Removed(Item item)
    {
        Remove(item);
    }

    public static bool ExistsHaulingJob(Item item, out HaulToZoneJob haulingJob)
    {
        foreach (var existingJob in JobManager.availableJobs)
        {
            if (existingJob is HaulToZoneJob haulToZoneJob)
            {
                if (haulToZoneJob.Item == item)
                {
                    haulingJob = haulToZoneJob;
                    return true;
                }
            }
        }

        haulingJob = null;
        return false;
    }

    private static bool IsItemHaulable(Item item)
    {
        switch (item.State)
        {
            case Item.ItemState.None:
                return false;

            case Item.ItemState.InHand:
                return false;

            case Item.ItemState.InInventory:
                return false;

            case Item.ItemState.InWorld:
                Vector3Int itemNode = item.GetNode();
                // TODO: this needs to be reworked, zones need to check also type
                return !ZoneManager.ZoneNodes.ContainsKey(itemNode);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void Check(Item item)
    {
        if (IsItemHaulable(item))
        {
            if (!ExistsHaulingJob(item, out HaulToZoneJob haulingJob))
            {
                Debug.Log($"[HaulableJobManager] Adding hauling job on item {item.ThingDef.Name}");
                JobManager.EnqueueJob(new HaulToZoneJob(item));
            }
        }
        else if (ExistsHaulingJob(item, out HaulToZoneJob haulingJob))
        {
            Debug.Log($"[HaulableJobManager] Removing hauling job on item {item.ThingDef.Name}");
            JobManager.DequeueJob(haulingJob);
        }
    }

    private static void Remove(Item item)
    {
        if (ExistsHaulingJob(item, out HaulToZoneJob haulingJob))
        {
            Debug.Log($"[HaulableJobManager] Removing hauling job on item {item.ThingDef.Name}");
            JobManager.DequeueJob(haulingJob);
        }
    }
}