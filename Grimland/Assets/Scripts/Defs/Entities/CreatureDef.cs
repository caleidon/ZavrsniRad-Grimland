using UnityEngine;

public abstract class CreatureDef : ThingDef
{
    public string Sprite { get; set; }
    public string Behavior { get; set; }

    public float BaseMovementSpeed { get; set; }

    // public Item ItemInHand { get; set; }
    public Inventory HandInventory { get; set; }

    public abstract Creature SpawnCreature(Vector3Int node);
    public abstract GameObject AssembleCreatureGO();
}