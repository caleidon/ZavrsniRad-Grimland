using BehaviorDesigner.Runtime;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Creature : Thing
{
    public enum PawnMovementUrgency
    {
        Relaxed,
        Walk,
        Jog,
        Sprint
    }

    public GameObject CreatureGO;
    public override bool IsRegionThing => true;
    public override ThingInteractionNode InteractionNode => ThingInteractionNode.Anywhere;
    public PawnMovementUrgency MovementUrgency { get; set; } = PawnMovementUrgency.Walk;
    public CreatureItemTracker ItemTracker { get; }


    public void InitializeBehaviorTree()
    {
        BehaviorTree bt = CreatureGO.GetComponent<BehaviorTree>();
        var btCreatureId = (SharedString)bt.GetVariable("creatureId");
        btCreatureId.Value = Id;
    }

    public void SetLocation(Vector3Int node)
    {
        CreatureGO.transform.position = node;
    }

    public Creature([CanBeNull] string id, ThingDef thingDef)
    {
        ThingDef = thingDef;

        GenerateThingId(id);

        ItemTracker = new CreatureItemTracker(this);
    }

    public override Vector3Int GetNode()
    {
        return Map.GridPosFromWorldPos(CreatureGO.transform.position);
    }
}