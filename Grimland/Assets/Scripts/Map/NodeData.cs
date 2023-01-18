using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeData
{
    public Vector3Int Node { get; }
    public bool IsWalkable { get; private set; }
    public bool IsDoor => false;

    public HashSet<Thing> Things { get; }

    // TODO: do we really need to convert to hashset?
    private HashSet<Tile> Tiles => new HashSet<Tile>(Things.OfType<Tile>());


    public NodeData(Vector3Int node)
    {
        Things = new HashSet<Thing>();
        Node = node;
    }

    // TODO: trygetitem and trygetblueprint can be replaced with a generic trygetT method
    public bool TryGetItem(out Item item)
    {
        Item potentialItem = Things.OfType<Item>().FirstOrDefault();
        if (potentialItem != null)
        {
            item = potentialItem;
            return true;
        }

        item = null;
        return false;
    }

    public bool TryGetBlueprint(out Blueprint blueprint)
    {
        Blueprint potentialBlueprint = Things.OfType<Blueprint>().FirstOrDefault();
        if (potentialBlueprint != null)
        {
            blueprint = potentialBlueprint;
            return true;
        }

        blueprint = null;
        return false;
    }

    public void AddThing(Thing thing)
    {
        Things.Add(thing);
        RefreshNodeData();
    }

    public void RemoveThing(Thing thing)
    {
        Things.Remove(thing);
        RefreshNodeData();
    }

    private void RefreshNodeData()
    {
        IsWalkable = Tiles.All(tile => ((TileDef) tile.ThingDef).IsWalkable);
    }

    public float GetFertility()
    {
        float totalFertility = 1f;

        foreach (var tile in Tiles)
        {
            if (tile is BaseTile baseTile)
            {
                totalFertility *= ((BaseTileDef) baseTile.ThingDef).Fertility;
            }
        }

        return totalFertility;
    }
}