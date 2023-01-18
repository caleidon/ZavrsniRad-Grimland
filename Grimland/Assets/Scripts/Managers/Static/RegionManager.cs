using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RegionManager
{
    public static Dictionary<string, Region> Regions { get; } = new Dictionary<string, Region>();
    public static Dictionary<Vector3Int, Region> RegionNodes { get; } = new Dictionary<Vector3Int, Region>();
    public static Dictionary<uint, HashSet<string>> Links { get; } = new Dictionary<uint, HashSet<string>>();

    public static void GenerateRegions()
    {
        foreach (var nodeData in NodeManager.AllNodeData())
        {
            if (!nodeData.IsWalkable) continue;

            /*if (node.IsDoor)
            {
                DoorRegion doorRegion = new DoorRegion(CreateRegionRectangle(node.Node.x, node.Node.y), new HashSet<Vector3Int> {new Vector3Int(node.Node.x, node.Node.y, 0)});
                continue;
            }*/

            if (nodeData.IsWalkable)
            {
                if (GetRegionFromNode(nodeData.Node, out Region region))
                {
                    continue;
                }

                var floodedTiles = FloodFill.FloodAllRectangleTiles(nodeData.Node);

                Region newRegion = new Region(CreateRegionRectangle(nodeData.Node.x, nodeData.Node.y), floodedTiles);
            }
        }
    }

    public static void GenerateRegionThings()
    {
        foreach (var nodeData in NodeManager.AllNodeData())
        {
            foreach (var thing in nodeData.Things)
            {
                if (thing.IsRegionThing)
                {
                    AddToRegionThings(thing, nodeData.Node);
                }
            }
        }
    }

    public static void UpdateRegion(Vector3Int node)
    {
        if (NodeManager.GetNodeDataAt(node).IsWalkable)
        {
            AddWalkableNode(node);
        }
        else
        {
            AddUnwalkableNode(node);
        }
    }

    private static void AddWalkableNode(Vector3Int node)
    {
        // In case we have to, we should only combine regions inside the same region rectangle
        Rect regionRect = CreateRegionRectangle(node.x, node.y);
        HashSet<Region> regionsToRework = FindRectangleRegions(node, regionRect);

        // We pass the current rooms to Room.JoinRooms so they can be combined
        HashSet<Region> currentRegions = FindAnyRegions(node);
        HashSet<Room> currentRooms = RoomManager.GetRoomsFromRegions(currentRegions);

        switch (regionsToRework.Count)
        {
            case 0: // No regions found in rectangle
                // Create a new region with the given node
                Region newRegion = new Region(regionRect, new HashSet<Vector3Int> {node});

                newRegion.UpdateLinks();
                newRegion.UpdateNeighborLinks(node);

                RoomManager.JoinRooms(newRegion, currentRooms);
                break;

            case 1: // Exactly one region found in rectangle
                // Add the given node to an existing region
                Region existingRegion = regionsToRework.First();
                existingRegion.AddNode(node);

                existingRegion.UpdateLinks();
                existingRegion.UpdateNeighborLinks(node);

                RoomManager.JoinRooms(existingRegion, currentRooms);
                break;

            default: // Multiple regions found in rectangle
                // Combine multiple regions into one new region
                HashSet<Vector3Int> newNodes = new HashSet<Vector3Int> {node};
                HashSet<uint> oldLinks = new HashSet<uint>();

                foreach (Region region in regionsToRework)
                {
                    // Fetch the old links from the current regions
                    // No need to calculate them again, so we just reuse them
                    foreach (uint link in region.CurrentLinks)
                    {
                        oldLinks.Add(link);
                    }

                    // Add all nodes to combined node pool for the combined region
                    newNodes.UnionWith(region.Nodes);
                    region.Delete();
                }

                Region combinedRegion = new Region(regionRect, newNodes, oldLinks);

                combinedRegion.UpdateLinks();
                combinedRegion.UpdateNeighborLinks(node);

                RoomManager.JoinRooms(combinedRegion, currentRooms);
                break;
        }
    }

    private static void AddUnwalkableNode(Vector3Int node)
    {
        if (!GetRegionFromNode(node, out Region targetedRegion))
        {
            Debug.Log("Returning here");
            return;
        }

        targetedRegion.RemoveNode(node);

        // After removing the node, also delete the region if it's empty
        if (targetedRegion.Nodes.Count == 0)
        {
            Room room = targetedRegion.GetRoom();

            targetedRegion.Delete();
            targetedRegion.UpdateNeighborLinks(node);

            // After deleting the region, it will remove itself from any rooms it's in
            // If the room it was in has no regions after this, also delete the room
            if (room.RegionIds.Count == 0)
            {
                room.Delete();
            }

            return;
        }

        Room targetedRoom = targetedRegion.GetRoom();

        SplitRegion(targetedRegion, targetedRoom);
        targetedRegion.UpdateNeighborLinks(node);

        RoomManager.SplitRooms(node, targetedRoom);
    }

    private static void SplitRegion(Region currentRegion, Room currentRoom)
    {
        while (true)
        {
            // Flood all tiles from a random point in this region
            HashSet<Vector3Int> floodedTiles = FloodFill.FloodRegionTiles(currentRegion);

            // If the total number of tiles doesn't match the number of flooded tiles
            // this room has been split, so we need to proceed
            bool fullyAccessible = currentRegion.Nodes.Count == floodedTiles.Count;

            if (fullyAccessible)
            {
                currentRegion.UpdateLinks();
                break;
            }

            // Store the initial tiles
            HashSet<Vector3Int> initialRegionNodes = currentRegion.Nodes;

            // Remove this smaller portion from all links, and assign it only the flooded tiles
            currentRegion.RemoveFromLinks();
            currentRegion.Nodes = floodedTiles;
            currentRegion.AddToRegionNodes(currentRegion.Nodes);
            currentRegion.UpdateLinks();

            // Create a new region with the remainder of the initial tiles (without the flooded ones)
            initialRegionNodes.ExceptWith(currentRegion.Nodes);
            Region newRegion = new Region(currentRegion.Rect, initialRegionNodes);

            // Add the new region to the current room, this is used to split rooms afterwards
            currentRoom.AddRegion(newRegion);

            // Repeat the process until the number of flooded tiles matches the total number of tiles for a region
            currentRegion = newRegion;
        }
    }

    public static HashSet<Region> FindAnyRegions(Vector3Int node)
    {
        HashSet<Region> foundRegions = new HashSet<Region>();

        foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
        {
            Vector3Int neighborPos = node + neighborOffset;

            if (GetRegionFromNode(neighborPos, out Region neighborRegion))
            {
                foundRegions.Add(neighborRegion);
            }
        }

        return foundRegions;
    }

    private static HashSet<Region> FindRectangleRegions(Vector3Int node, Rect regionRect)
    {
        HashSet<Region> foundRegions = new HashSet<Region>();

        foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
        {
            Vector3Int neighborPos = node + neighborOffset;

            if (!regionRect.Contains(neighborPos))
            {
                continue;
            }

            if (GetRegionFromNode(neighborPos, out Region neighborRegion) && !(neighborRegion is DoorRegion))
            {
                foundRegions.Add(neighborRegion);
            }
        }

        return foundRegions;
    }

    public static Rect CreateRegionRectangle(int x, int y)
    {
        int regionX = x / Settings.REGION_SIZE * Settings.REGION_SIZE;
        int regionY = y / Settings.REGION_SIZE * Settings.REGION_SIZE;

        int rectWidth = regionX + Settings.REGION_SIZE > Map.Instance.Size.x ? Map.Instance.Size.x - regionX : Settings.REGION_SIZE;
        int rectHeight = regionY + Settings.REGION_SIZE > Map.Instance.Size.y ? Map.Instance.Size.y - regionY : Settings.REGION_SIZE;

        return new Rect(regionX, regionY, rectWidth, rectHeight);
    }

    public static bool GetRegionFromNode(Vector3Int node, out Region foundRegion)
    {
        if (RegionNodes.TryGetValue(node, out Region region))
        {
            foundRegion = region;
            return true;
        }

        foundRegion = null;
        return false;
    }

    public static void AddToRegionThings(Thing thing, Vector3Int node)
    {
        foreach (var offset in DirectionUtils.AllOffsets)
        {
            Vector3Int offsetNode = node + offset;
            if (GetRegionFromNode(offsetNode, out Region region))
            {
                region.RegionThings.Add(thing);
            }
        }
    }

    public static void RemoveFromRegionThings(Thing thing, Vector3Int node)
    {
        foreach (var offset in DirectionUtils.AllOffsets)
        {
            Vector3Int offsetNode = node + offset;
            if (GetRegionFromNode(offsetNode, out Region region))
            {
                region.RegionThings.Remove(thing);
            }
        }
    }

    // TODO: this can probably be rewritten by a generic method that floods regions and does something in between each region
    public static bool ExistsItemAmountInRegions(Region startingRegion, ItemDef itemDef, int amountNeeded)
    {
        int amountFound = 0;

        amountFound += startingRegion.ItemAmountInRegionThings(itemDef);

        if (amountFound >= amountNeeded)
        {
            return true;
        }

        Queue<Region> queue = new Queue<Region>();
        HashSet<Region> visitedRegions = new HashSet<Region>();

        queue.Enqueue(startingRegion);
        visitedRegions.Add(startingRegion);

        while (queue.Count > 0)
        {
            Region region = queue.Dequeue();

            foreach (Region neighborRegion in region.GetNeighbors())
            {
                if (neighborRegion == null || visitedRegions.Contains(neighborRegion) || neighborRegion is DoorRegion)
                {
                    continue;
                }

                amountFound += neighborRegion.ItemAmountInRegionThings(itemDef);

                if (amountFound >= amountNeeded)
                {
                    return true;
                }

                visitedRegions.Add(neighborRegion);
                queue.Enqueue(neighborRegion);
            }
        }

        return amountFound >= amountNeeded;
    }

    public static bool TryFindNearestItem(Region startingRegion, ItemDef itemDef, out Item foundItem)
    {
        if (startingRegion.TryFindItemInRegionThings(itemDef, out Item foundItem1))
        {
            foundItem = foundItem1;
            return true;
        }

        Queue<Region> queue = new Queue<Region>();
        HashSet<Region> visitedRegions = new HashSet<Region>();

        queue.Enqueue(startingRegion);
        visitedRegions.Add(startingRegion);

        while (queue.Count > 0)
        {
            Region region = queue.Dequeue();

            foreach (Region neighborRegion in region.GetNeighbors())
            {
                if (neighborRegion == null || neighborRegion is DoorRegion)
                {
                    continue;
                }

                if (visitedRegions.Contains(neighborRegion))
                {
                    continue;
                }

                if (neighborRegion.TryFindItemInRegionThings(itemDef, out Item foundItem2))
                {
                    foundItem = foundItem2;
                    return true;
                }

                visitedRegions.Add(neighborRegion);
                queue.Enqueue(neighborRegion);
            }
        }

        foundItem = null;
        return false;
    }

    public static void Reset()
    {
        Regions.Clear();
        RegionNodes.Clear();
        Links.Clear();
    }
}