using System.Collections.Generic;
using UnityEngine;

public class DoorRegion : Region
{
    public DoorRegion(Rect rect, HashSet<Vector3Int> regionNodes = null, HashSet<uint> links = null) : base(rect, regionNodes, links)
    {
        string regionID = IdManager.GenerateGUID();

        Id = regionID;
        Rect = rect;

        Nodes = regionNodes ?? new HashSet<Vector3Int>();
        CurrentLinks = links ?? new HashSet<uint>();
        RegionThings = new HashSet<Thing>();

        RegionManager.Regions.Add(regionID, this);
    }

    public static void Place(Vector3Int node, Rect regionRect)
    {
        //Map.Instance.nodeData[node.x, node.y].type = NodeData.Type.Door;
        // TODO: dont set door, NodeData will know if its a door based on its ITiles
        DoorRegion doorRegion = new DoorRegion(regionRect, new HashSet<Vector3Int> { node });

        doorRegion.UpdateLinks();
        doorRegion.UpdateNeighborLinks(node);

        Room doorRoom = new Room(new HashSet<Region> { doorRegion });
        doorRoom.UpdateNeighbors();

        foreach (Region neighborRegion in doorRegion.GetNeighbors())
        {
            Room roomToUpdate = neighborRegion.GetRoom();
            roomToUpdate.UpdateNeighbors();
        }
    }

    public static void Remove(Vector3Int node)
    {
        // DoorRegion doorRegionToRemove = (DoorRegion) Map.Instance.GetRegionFromNode(node);
        // doorRegionToRemove.Delete();
        // Map.Instance.RemoveGameObject(node);
        // AddNode(node);
    }
}