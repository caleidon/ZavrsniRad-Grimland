using System.Collections.Generic;
using UnityEngine;

public static class NodeManager
{
    private static NodeData[,] NodeData { get; set; }

    public static NodeData GetNodeDataAt(Vector3Int node)
    {
        return NodeData[node.x, node.y];
    }

    public static IEnumerable<NodeData> AllNodeData()
    {
        foreach (var node in Map.AllNodes())
        {
            yield return GetNodeDataAt(node);
        }
    }

    public static void Reset(Vector2Int mapSize)
    {
        NodeData = new NodeData[mapSize.x, mapSize.y];

        foreach (var node in Map.AllNodes())
        {
            NodeData[node.x, node.y] = new NodeData(node);
        }
    }
}

public class NodeManagerSaver
{
    public List<ISaver> ThingSavers { get; set; }
    public string DeflatedBase { get; set; }
    public string DeflatedFloor { get; set; }

    public NodeManagerSaver()
    {
        GetDeflationStrings();
        GetThingSavers();
    }

    private void GetThingSavers()
    {
        List<ISaver> thingSavers = new List<ISaver>();

        foreach (var thing in IdManager.GetAllThings())
        {
            if (thing is ICanBeSaved thingSaver)
            {
                thingSavers.Add(thingSaver.GetSaver());
            }
        }

        ThingSavers = thingSavers;
    }

    private void GetDeflationStrings()
    {
        string[] deflatedBaseArray = new string[Map.Instance.Size.x * Map.Instance.Size.y].PopulateArray("-/");
        string[] deflatedFloorArray = new string[Map.Instance.Size.x * Map.Instance.Size.y].PopulateArray("-/");

        foreach (var nodeData in NodeManager.AllNodeData())
        {
            int currentIndex = nodeData.Node.y * Map.Instance.Size.x + nodeData.Node.x;

            foreach (var thing in nodeData.Things)
            {
                switch (thing)
                {
                    case BaseTile baseTileData:
                        deflatedBaseArray[currentIndex] = baseTileData.GetDeflationString();
                        break;
                    case FloorTile floorTileData:
                        deflatedFloorArray[currentIndex] = floorTileData.GetDeflationString();
                        break;
                }
            }
        }

        DeflatedBase = SavingUtils.Deflate(deflatedBaseArray);
        DeflatedFloor = SavingUtils.Deflate(deflatedFloorArray);
    }
}