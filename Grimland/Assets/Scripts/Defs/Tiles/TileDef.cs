using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileDef : ThingDef
{
    public bool IsWalkable => MoveSpeedMultiplier > 0;
    public float MoveSpeedMultiplier { get; set; }

    public abstract TileBase GetTile();

    public abstract Tile Place(Vector3Int node, Tile existingTile, bool loadingMode = false);
}