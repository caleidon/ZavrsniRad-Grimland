using Pathfinding;
using UnityEngine;

public static class PathfindingManager
{
    // PATHFINDING TODOS:
    // TODO: if trying to pathfind to an obstacle, instead pathfind to the nearest free block (for intuitiveness)
    // TODO: fix astar going through walls glitch when tile is placed
    // TODO: Avoid sending repathing event to all pawns when a block gets changed, instead inform only those whose path contains that region

    private static AstarData _graphData;
    private static GridGraph _gridGraph;

    public static void InitializeGstar()
    {
        _graphData = AstarPath.active.data;
        _gridGraph = _graphData.gridGraph;

        _gridGraph.center = new Vector3((float)Map.Instance.Size.x / 2, (float)Map.Instance.Size.y / 2, 0) - new Vector3(0.5f, 0.5f, 0f);
        _gridGraph.SetDimensions(Map.Instance.Size.x, Map.Instance.Size.y, 1);
        _gridGraph.cutCorners = false;
        _gridGraph.neighbours = NumNeighbours.Eight;

        AstarPath.active.Scan();
        RescanGraph();
    }

    private static void RescanGraph()
    {
        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            for (int y = 0; y < _gridGraph.depth; y++)
            {
                for (int x = 0; x < _gridGraph.width; x++)
                {
                    var node = _gridGraph.GetNode(x, y);
                    node.Walkable = NodeManager.GetNodeDataAt(new Vector3Int(x, y, 0)).IsWalkable;
                }
            }

            _gridGraph.GetNodes(node => _gridGraph.CalculateConnections((GridNodeBase)node));
        }));
    }

    public static void UpdateGraphPosition(Vector3Int node)
    {
        AstarPath.active.AddWorkItem(ctx =>
        {
            _gridGraph.GetNode(node.x, node.y).Walkable = NodeManager.GetNodeDataAt(node).IsWalkable;
            _gridGraph.CalculateConnectionsForCellAndNeighbours(node.x, node.y);
        });

        AstarPath.active.FlushWorkItems();
    }
}