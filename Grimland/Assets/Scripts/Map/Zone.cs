using System;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
    // TODO: when a zone is updated (mostly deleted), check all items that were in it for hauling
    // Also, when an item is picked up from zone, re-run the merging algorithm
    public string Id { get; set; }
    public HashSet<Vector3Int> Nodes { get; set; } = new HashSet<Vector3Int>();

    public List<string> AllowedTags { get; set; }

    public event Action OnZoneChanged;

    public Zone(HashSet<Vector3Int> nodes)
    {
        Id = IdManager.GenerateZoneID();

        foreach (var node in nodes)
        {
            AddNode(node);

            var marker = new GameObject();
            marker.transform.position = node;
            var sr = marker.AddComponent<SpriteRenderer>();
            sr.sprite = DefManager.Sprites["zonemarker"];
            sr.sortingOrder = 25;
            sr.color = new Color(1, 1, 1, 0.1f);
        }

        ZoneManager.Zones.Add(Id, this);
    }

    public void AddNode(Vector3Int node)
    {
        Nodes.Add(node);

        if (!ZoneManager.ZoneNodes.ContainsKey(node))
        {
            ZoneManager.ZoneNodes.Add(node, this);
        }

        OnZoneChanged?.Invoke();
    }

    public void RemoveNode(Vector3Int node)
    {
        Nodes.Remove(node);

        if (ZoneManager.ZoneNodes.ContainsKey(node))
        {
            ZoneManager.ZoneNodes.Remove(node);
        }

        OnZoneChanged?.Invoke();

        if (Nodes.Count == 0)
        {
            Delete();
        }
    }

    private void Delete()
    {
        foreach (var zoneNode in Nodes)
        {
            if (ZoneManager.ZoneNodes.ContainsKey(zoneNode))
            {
                ZoneManager.ZoneNodes.Remove(zoneNode);
            }
        }

        ZoneManager.Zones.Remove(Id);

        OnZoneChanged?.Invoke();
    }

    public bool TryGetUnreservedItemWithFreeSpace(Item item, out Item foundItem, out int freeSpace)
    {
        foreach (var node in Nodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            if (nodeData.TryGetItem(out Item itemInZone))
            {
                if (!item.HasSameDefAs(itemInZone) || itemInZone.GetRemainingSpace() <= 0 || !ReservationManager.CanReserve(itemInZone, ReservationType.HaulTo, out Creature owner))
                {
                    continue;
                }

                foundItem = itemInZone;
                freeSpace = itemInZone.GetRemainingSpace();
                return true;
            }

            // TODO: Check for chestTile / inventory here
        }

        foundItem = null;
        freeSpace = 0;
        return false;
    }

    public bool TryGetEmptyUnreservedNode(out Vector3Int emptyNode)
    {
        foreach (var node in Nodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            if (nodeData.TryGetItem(out Item item) || !HaulReservationManager.CanReserve(node, out Creature owner))
            {
                continue;
            }

            // TODO: check for chesttile / inventory here

            emptyNode = node;
            return true;
        }

        emptyNode = Vector3Int.zero;
        return false;
    }

    public int GetTotalCapacityForItem(Item item)
    {
        int availableSpace = 0;

        foreach (var node in Nodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            // TODO: add support for inventory
            if (nodeData.TryGetItem(out Item itemInZone))
            {
                if (item.HasSameDefAs(itemInZone))
                {
                    availableSpace += itemInZone.GetRemainingSpace();
                }
            }
            else
            {
                availableSpace += item.GetItemDef().MaxStackSize;
            }
        }

        return availableSpace;
    }

    public HaulDestination GetHaulDestination(Item item)
    {
        if (TryGetUnreservedItemWithFreeSpace(item, out Item foundItem, out int freeSpace))
        {
            return new HaulDestination(foundItem, freeSpace);
        }

        if (TryGetEmptyUnreservedNode(out Vector3Int emptyNode))
        {
            return new HaulDestination(emptyNode);
        }

        return new HaulDestination();
    }
}