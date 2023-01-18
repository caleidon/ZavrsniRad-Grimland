using UnityEngine;

public abstract class Tile : Thing
{
    protected Vector3Int Node { get; set; }
    public override ThingInteractionNode InteractionNode => ThingInteractionNode.Sides;

    protected Tile(Vector3Int node, ThingDef tileDef)
    {
        Node = node;
        ThingDef = tileDef;
    }

    public override Vector3Int GetNode()
    {
        return Node;
    }

    public TileDef GetTileDef()
    {
        return ThingDef as TileDef;
    }
}