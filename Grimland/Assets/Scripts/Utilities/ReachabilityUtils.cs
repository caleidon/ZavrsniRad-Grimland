using System;
using Pathfinding;
using UnityEngine;

public static class ReachabilityUtils
{
    public static bool IsLocationReachableFromRegion(Vector3Int node, Region region)
    {
        if (!RegionManager.GetRegionFromNode(node, out Region remoteRegion))
        {
            return false;
        }

        Room remoteRoom = remoteRegion.GetRoom();

        return region.GetRoom().IsRoomAccessible(remoteRoom);
    }

    public static bool TryGetClosestInteractionNode(Thing thing, out Vector3Int interactionNode)
    {
        // TODO: this won't return closest interaction node, but the first one it can find
        // TODO do we check for reserved node here?

        Vector3Int thingCenter = thing.GetNode();

        switch (thing.InteractionNode)
        {
            case Thing.ThingInteractionNode.None:
                break;

            case Thing.ThingInteractionNode.Center:
                if (IsInteractionNodeReachable(thingCenter))
                {
                    interactionNode = thingCenter;
                    return true;
                }

                break;

            case Thing.ThingInteractionNode.Sides:
                foreach (var offset in DirectionUtils.SideOffsets)
                {
                    Vector3Int inspectionNode = thingCenter + offset;
                    if (IsInteractionNodeReachable(inspectionNode))
                    {
                        interactionNode = inspectionNode;
                        return true;
                    }
                }

                break;

            case Thing.ThingInteractionNode.Corners:
                foreach (var offset in DirectionUtils.CornerOffsets)
                {
                    Vector3Int inspectionNode = thingCenter + offset;
                    if (IsInteractionNodeReachable(inspectionNode))
                    {
                        interactionNode = inspectionNode;
                        return true;
                    }
                }

                break;

            case Thing.ThingInteractionNode.InteractionNode:
                // TODO: implement interaction node for structuretiles
                break;

            case Thing.ThingInteractionNode.Anywhere:
                foreach (var offset in DirectionUtils.AllOffsets)
                {
                    Vector3Int inspectionNode = thingCenter + offset;
                    if (IsInteractionNodeReachable(inspectionNode))
                    {
                        interactionNode = inspectionNode;
                        return true;
                    }
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        interactionNode = Vector3Int.zero;
        return false;
    }

    private static bool IsInteractionNodeReachable(Vector3Int node)
    {
        NodeData nodeData = NodeManager.GetNodeDataAt(node);
        return nodeData.IsWalkable && !nodeData.IsDoor;
    }

    public static bool IsPathPossible(Vector3Int start, Vector3Int end)
    {
        // TODO: this completely ignores doors

        GraphNode node1 = AstarPath.active.GetNearest(start, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(end, NNConstraint.Default).node;

        return PathUtilities.IsPathPossible(node1, node2);
    }
}