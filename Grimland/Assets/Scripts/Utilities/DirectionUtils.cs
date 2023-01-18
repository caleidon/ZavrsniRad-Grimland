using UnityEngine;

public static class DirectionUtils
{
    public static readonly Vector3Int[] SideOffsets =
    {
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.up,
        Vector3Int.right
    };

    public static readonly Vector3Int[] CornerOffsets =
    {
        new Vector3Int(-1, -1, 0), // Bottom left,
        new Vector3Int(-1, 1, 0), // Top left
        new Vector3Int(1, 1, 0), // Top right
        new Vector3Int(1, -1, 0) // Bottom right
    };

    public static readonly Vector3Int[] AllOffsets =
    {
        Vector3Int.down,
        new Vector3Int(-1, -1, 0), // Bottom left
        Vector3Int.left,
        new Vector3Int(-1, 1, 0), // Top left
        Vector3Int.up,
        new Vector3Int(1, 1, 0), // Top right
        Vector3Int.right,
        new Vector3Int(1, -1, 0) // Bottom right
    };
}