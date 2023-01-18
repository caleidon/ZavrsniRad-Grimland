using UnityEngine;

public abstract class ItemDef : ThingDef
{
    public UnityEngine.Tilemaps.Tile Tile { get; set; }
    public float MoveSpeedMultiplier { get; set; }
    public int MaxStackSize { get; set; }
    public int MaxHealth { get; set; }
    public abstract Item Create(int amount);

    // public abstract Item PlaceInWorld(Vector3Int node, Item item, bool updateRegion);

    public Item PlaceInWorld(Vector3Int node, Item item, bool updateRegion)
    {
        item.PlaceInWorld(node);

        NodeManager.GetNodeDataAt(node).AddThing(item);
        RegionManager.AddToRegionThings(item, node);
        HaulableJobManager.Notify_Spawned(item);

        Map.Instance.SetTile(node, Tile, Map.TilemapType.ItemTilemap, updateRegion);
        return item;
    }

    public bool IsSameAs(ItemDef itemDef)
    {
        return Name == itemDef.Name;
    }
}