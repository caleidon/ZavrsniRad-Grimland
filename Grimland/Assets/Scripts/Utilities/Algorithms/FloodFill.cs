using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FloodFill
{
    public static HashSet<Region> FloodAllRegions(Region startingRegion)
    {
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

                visitedRegions.Add(neighborRegion);
                queue.Enqueue(neighborRegion);
            }
        }

        return visitedRegions;
    }

    public static HashSet<Vector3Int> FloodAllRectangleTiles(Vector3Int startingNode)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();

        queue.Enqueue(startingNode);
        visitedNodes.Add(startingNode);

        Rect regionRect = RegionManager.CreateRegionRectangle(startingNode.x, startingNode.y);

        while (queue.Count > 0)
        {
            Vector3Int tile = queue.Dequeue();

            foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
            {
                Vector3Int neighborPos = tile + neighborOffset;

                // If a neighbor is outside of the region rectangle, was already examined or is an obstacle, ignore it
                if (!Map.Contains(neighborPos) || !NodeManager.GetNodeDataAt(neighborPos).IsWalkable || visitedNodes.Contains(neighborPos) ||
                    NodeManager.GetNodeDataAt(neighborPos).IsDoor || !regionRect.Contains(neighborPos))
                {
                    // TODO questionable that here we check for door, shouldn't we check for it somewhere else?
                    continue;
                }

                visitedNodes.Add(neighborPos);
                queue.Enqueue(neighborPos);
            }
        }

        return visitedNodes;
    }

    // TODO: add option to ignore current tile
    public static HashSet<Vector3Int> FloodAllTiles(Vector3Int startingNode, int maxDistanceFromCenter = int.MaxValue)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();

        queue.Enqueue(startingNode);
        visitedNodes.Add(startingNode);

        while (queue.Count > 0)
        {
            Vector3Int tile = queue.Dequeue();

            var xDistance = Mathf.Abs(tile.x - startingNode.x);
            var yDistance = Mathf.Abs(tile.y - startingNode.y);

            if (xDistance >= maxDistanceFromCenter || yDistance >= maxDistanceFromCenter)
            {
                break;
            }

            foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
            {
                Vector3Int neighborPos = tile + neighborOffset;

                // If a neighbor is outside of the region rectangle, was already examined or is an obstacle, ignore it
                if (!Map.Contains(neighborPos) || !NodeManager.GetNodeDataAt(neighborPos).IsWalkable ||
                    visitedNodes.Contains(neighborPos) || NodeManager.GetNodeDataAt(neighborPos).IsDoor)
                {
                    // TODO questionable that here we check for door, shouldn't we check for it somewhere else?
                    continue;
                }

                visitedNodes.Add(neighborPos);
                queue.Enqueue(neighborPos);
            }
        }

        return visitedNodes;
    }


    public static bool FloodFindClosestNode(Vector3Int startingNode, out Vector3Int walkableNode, int searchRange = 2)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();

        queue.Enqueue(startingNode);
        visitedNodes.Add(startingNode);

        while (queue.Count > 0)
        {
            Vector3Int tile = queue.Dequeue();

            foreach (Vector3Int neighborOffset in DirectionUtils.AllOffsets)
            {
                Vector3Int neighborNode = tile + neighborOffset;

                if (!Map.Contains(neighborNode) || Mathf.Abs(neighborNode.x - startingNode.x) > searchRange ||
                    Mathf.Abs(neighborNode.y - startingNode.y) > searchRange)
                {
                    Debug.Log($"NODE {neighborNode} was outside of search range");
                    continue;
                }

                Debug.Log($"EXAMINING NODE {neighborNode}");
                // TODO lik svejedno ide closer jer ne checka dal oopce moze, check rooms likely

                // If a neighbor was already examined, is outside of the map or is an obstacle, ignore it
                if (visitedNodes.Contains(neighborNode))
                {
                    continue;
                }

                if (NodeManager.GetNodeDataAt(neighborNode).IsWalkable || NodeManager.GetNodeDataAt(neighborNode).IsDoor)
                {
                    walkableNode = neighborNode;
                    return true;
                }

                visitedNodes.Add(neighborNode);
                queue.Enqueue(neighborNode);
            }
        }

        walkableNode = Vector3Int.zero;
        return false;
    }

    public static HashSet<Vector3Int> FloodRegionTiles(Region region)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();

        var startingNode = region.Nodes.First();

        queue.Enqueue(startingNode);
        visitedNodes.Add(startingNode);

        while (queue.Count > 0)
        {
            Vector3Int tile = queue.Dequeue();

            foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
            {
                Vector3Int neighborPos = tile + neighborOffset;

                // If a neighbor is outside of the region rectangle, was already examined or is an obstacle, ignore it
                if (visitedNodes.Contains(neighborPos) || !region.Nodes.Contains(neighborPos))
                {
                    continue;
                }

                visitedNodes.Add(neighborPos);
                queue.Enqueue(neighborPos);
            }
        }

        return visitedNodes;
    }
}