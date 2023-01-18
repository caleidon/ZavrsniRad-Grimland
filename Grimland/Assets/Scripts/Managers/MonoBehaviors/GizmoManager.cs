using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GizmoManager : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        DrawZones();

        if (SelectionManager.Instance.currentMode == SelectionManager.Mode.BoxSelection)
        {
            DrawSelectedTiles();
        }

        if (Keyboard.current[Key.Z].isPressed)
        {
            DrawLinks();
        }

        if (Keyboard.current[Key.X].isPressed)
        {
            DrawWireframe();
        }

        if (Keyboard.current[Key.C].isPressed)
        {
            DrawRegions();
        }

        if (Keyboard.current[Key.V].isPressed)
        {
            DrawRooms();
        }

        if (Keyboard.current[Key.B].isPressed)
        {
            DrawRegionThings();
        }

        if (Keyboard.current[Key.M].isPressed)
        {
            DrawTestTiles();
        }
    }

    // TODO: show this on canvas, not here
    /*private static void DrawSelectedTiles()
    {
        Gizmos.color = new Color(0.5f, 0.4f, 1f, 0.3f);
        foreach (var selectedNode in SelectionManager.selectedNodes) { Gizmos.DrawCube(new Vector3(selectedNode.x, selectedNode.y, 0), new Vector3(1, 1, 1)); }
    }*/

    private static void DrawSelectedTiles()
    {
        Gizmos.color = new Color(0.5f, 0.4f, 1f, 0.3f);
        foreach (var selectedNode in SelectionManager.selectedNodes)
        {
            Gizmos.DrawCube(new Vector3(selectedNode.x, selectedNode.y, 0), new Vector3(1, 1, 1));
        }
    }

    private static void DrawRooms()
    {
        Vector3Int gridNode = Map.GridPosFromMousePos();

        if (!RegionManager.GetRegionFromNode(gridNode, out Region selectedRegion))
        {
            return;
        }

        Room selectedRoom = selectedRegion?.GetRoom();
        if (selectedRoom == null)
        {
            return;
        }

        Utilities.ShowGizmoText(selectedRoom.Id.Substring(0, 5), selectedRegion.Nodes.First(), Color.white, 20);
        Gizmos.color = new Color(0, 0.9f, 0.9f, 0.4f);
        foreach (string regionID in selectedRoom.RegionIds)
        {
            foreach (Vector3Int node in RegionManager.Regions[regionID].Nodes)
            {
                Gizmos.DrawCube(new Vector3(node.x, node.y, 0), new Vector3(1, 1, 1));
            }
        }

        Gizmos.color = new Color(1, 0.92f, 0.016f, 0.2f);
        foreach (string neighborRoomID in selectedRoom.NeighborRoomIds)
        {
            Room neighborRoom = RoomManager.Rooms[neighborRoomID];
            string neighborRoomRegionID = neighborRoom.RegionIds.First();
            Region neighborRoomRegion = RegionManager.Regions[neighborRoomRegionID];

            foreach (string regionID in neighborRoom.RegionIds)
            {
                foreach (Vector3Int node in RegionManager.Regions[regionID].Nodes)
                {
                    Gizmos.DrawCube(new Vector3(node.x, node.y, 0), new Vector3(1, 1, 1));
                }
            }

            Utilities.ShowGizmoText(neighborRoom.Id.Substring(0, 5), neighborRoomRegion.Nodes.First(), Color.red);
        }
    }

    private static void DrawRegions()
    {
        Vector3Int gridNode = Map.GridPosFromMousePos();

        if (!RegionManager.GetRegionFromNode(gridNode, out Region selectedRegion))
        {
            return;
        }

        Utilities.ShowGizmoText(selectedRegion.Id.Substring(0, 5), selectedRegion.Nodes.First(), Color.white, 20);

        if (selectedRegion.GetNeighbors().Contains(selectedRegion))
        {
            Utilities.ShowGizmoText("REGION IS NEIGHBOR WITH ITSELF", selectedRegion.Nodes.First(), Color.red, 30);
        }


        foreach (Vector3Int node in selectedRegion.Nodes)
        {
            DrawOutline(node, selectedRegion, new Color(0, 1, 0, 0.8f));
        }

        foreach (Region neighborRegion in selectedRegion.GetNeighbors())
        {
            Utilities.ShowGizmoText(neighborRegion.Id.Substring(0, 5), neighborRegion.Nodes.First(), Color.red);

            foreach (Vector3Int node in neighborRegion.Nodes)
            {
                DrawOutline(node, neighborRegion, new Color(1, 1, 1, 0.2f));
            }
        }
    }

    private static void DrawOutline(Vector3Int node, Region region, Color color)
    {
        Gizmos.color = color;

        foreach (var offset in DirectionUtils.SideOffsets)
        {
            Vector3Int neighbor = node + offset;

            if (!region.Nodes.Contains(neighbor))
            {
                float xOffset = 0;
                float yOffset = 0;

                if (offset == Vector3Int.left)
                {
                    xOffset = -0.35f;
                }
                else if (offset == Vector3Int.right)
                {
                    xOffset = 0.35f;
                }

                if (offset == Vector3Int.up)
                {
                    yOffset = 0.35f;
                }
                else if (offset == Vector3Int.down)
                {
                    yOffset = -0.35f;
                }

                Gizmos.DrawCube(new Vector3(node.x + xOffset, node.y + yOffset, 0), new Vector3(0.3f + Mathf.Abs(yOffset) * 2, 0.3f + Mathf.Abs(xOffset) * 2, 1f));
            }
        }
    }

    private static void DrawRegionThings()
    {
        Vector3Int gridNode = Map.GridPosFromMousePos();

        if (!RegionManager.GetRegionFromNode(gridNode, out Region selectedRegion))
        {
            return;
        }

        foreach (var thing in selectedRegion.RegionThings)
        {
            if (thing.IsRegionThing)
            {
                Vector3 location = thing.GetNode();
                Gizmos.color = Utilities.ColorFromHash(thing.GetHashCode());
                Gizmos.DrawSphere(new Vector3(location.x, location.y, 0), 0.3f);
            }
        }
    }

    private static void DrawWireframe()
    {
        foreach (Region region in RegionManager.Regions.Values)
        {
            // Draw the node of the hovered tile
            Vector3Int gridNode = Map.GridPosFromMousePos();
            Utilities.ShowGizmoText(new Vector2Int(gridNode.x, gridNode.y).ToString(), gridNode - new Vector3Int(0, 1, 0), Color.white, 15, false);

            // Draw region squares
            Gizmos.color = Color.green;
            foreach (Vector3Int node in region.Nodes)
            {
                Gizmos.DrawWireCube(new Vector3(node.x, node.y, 0), new Vector3(1, 1, 1));
            }

            // Draw original region rectangles
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(region.Rect.center.x - 0.5f, region.Rect.center.y - 0.5f, 0), new Vector3(region.Rect.width, region.Rect.height, 1));
        }

        /*foreach (Region region in RegionManager.Regions.Values)
        {
            // Draw the node of the hovered tile
            Vector3Int gridNode = Utilities.GridPosFromMousePos();
            //Utilities.ShowGizmoText(new Vector2Int(gridNode.x, gridNode.y).ToString(), gridNode - new Vector3Int(0, 1, 0), Color.white, 15, false);

            // Draw region squares
            foreach (Vector3Int node in region.RegionNodes) { Draw.WireRectangleXZ(new float3(node.x, node.y, 0), new float2(1, 1), Color.green); }

            // Draw original region rectangles
            Draw.WireRectangleXZ(new float3(region.RegionRect.center.x - 0.5f, region.RegionRect.center.y - 0.5f, 0), new float2(region.RegionRect.width, region.RegionRect.height), Color.red);
        }

        Draw.ingame.WireRectangle(new float3(-1, -1, -1), Quaternion.Euler(-90, 0, 0), new float2(10, 10), Color.magenta);
        Draw.ingame.WireRectangle(new float3(-1, -1, -1), Quaternion.Euler(0, -90, 0), new float2(10, 10), Color.magenta);
        Draw.ingame.WireRectangle(new float3(-1, -1, -1), Quaternion.Euler(0, 0, -90), new float2(10, 10), Color.magenta);*/
    }

    private static void DrawLinks()
    {
        foreach (Region region in RegionManager.Regions.Values)
        {
            Gizmos.color = Color.magenta;

            var linkTiles = region.FindLinkTiles();

            foreach (var linkTile in linkTiles)
            {
                Gizmos.DrawCube(new Vector3(linkTile.node.x, linkTile.node.y, 0), new Vector3(1, 1, 1));
            }
        }
    }

    private static void DrawZones()
    {
        if (Map.Instance == null)
        {
            return;
        }

        // Gizmos.color = new Color(0, 0.4f, 0.8f, 0.2f);

        foreach (var zone in ZoneManager.Zones.Values)
        {
            Color color = Utilities.ColorFromHash(zone.GetHashCode());
            color.a = 0.2f;
            Gizmos.color = color;
            foreach (var node in zone.Nodes)
            {
                Gizmos.DrawCube(new Vector3(node.x, node.y, 0), new Vector3(1, 1, 1));
            }
        }
    }

    public static List<Vector3Int> testNodes = new List<Vector3Int>();

    private static void DrawTestTiles()
    {
        Gizmos.color = Color.red;
        foreach (var node in testNodes)
        {
            Gizmos.DrawCube(new Vector3(node.x, node.y, 0), new Vector3(1, 1, 1));
        }
    }
}