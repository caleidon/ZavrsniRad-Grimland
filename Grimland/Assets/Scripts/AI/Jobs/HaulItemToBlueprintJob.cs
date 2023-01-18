using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class HaulItemToBlueprintJob : Job
{
    public Blueprint Blueprint { get; set; }
    public override bool Suspendable => false;
    public override bool AutoRestart => false;
    public override Creature.PawnMovementUrgency MovementUrgency => Creature.PawnMovementUrgency.Jog;

    public List<Item> ItemsToPickUp = new List<Item>();
    public List<Blueprint> BlueprintsToHaulTo = new List<Blueprint>();


    public HaulItemToBlueprintJob(Blueprint blueprint)
    {
        Blueprint = blueprint;
        KnownLocations.Add(Blueprint.GetNode());
    }


    public override bool CanBeDoneBy(Creature creature)
    {
        return true;
    }

    public override bool AreRequirementsFulfilled(Creature creature)
    {
        ItemDefCount itemDefCount = Blueprint.GetNextRequiredMaterialAmount();

        if (itemDefCount != null)
        {
            if (RegionManager.GetRegionFromNode(creature.GetNode(), out Region foundRegion))
            {
                if (!RegionManager.ExistsItemAmountInRegions(foundRegion, itemDefCount.ItemDef, itemDefCount.Count))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override bool MakeReservations(Creature creature)
    {
        ItemsToPickUp.Clear();
        BlueprintsToHaulTo.Clear();

        ItemDefCount itemDefCount = Blueprint.GetNextRequiredMaterialAmount();

        if (!RegionManager.GetRegionFromNode(creature.GetNode(), out Region foundRegion))
        {
            return false;
        }

        if (!RegionManager.TryFindNearestItem(foundRegion, itemDefCount.ItemDef, out Item foundItem))
        {
            return false;
        }

        if (!ReservationManager.TryReserve(creature, foundItem, this, ReservationType.HaulFrom))
        {
            return false;
        }

        if (!ReservationManager.TryReserve(creature, Blueprint, this, ReservationType.HaulTo))
        {
            return false;
        }

        ItemsToPickUp.Add(foundItem);
        BlueprintsToHaulTo.Add(Blueprint);

        return true;
    }

    public override void OnJobFailed(Creature creature)
    {
        base.OnJobFailed(creature);

        if (creature.ItemTracker.HasItemInHand)
        {
            Debug.Log($"[HaulItemToBlueprintFailed] Item dropped: {creature.ItemTracker.HeldItem} with amount {creature.ItemTracker.HeldItem.Amount}");
            creature.ItemTracker.DropItemOnFloor();
        }

        BehaviorTree bt = creature.CreatureGO.GetComponent<BehaviorTree>();
        // TODO: The entire tree will share "ItemId" - maybe rename it to something more specific?
        // TODO: make behavior tree load jobchooser so we can just fetch it here
        var lastItemId = (SharedString) bt.GetVariable("ItemToTakeId");
        HaulableJobManager.Notify_Spawned((Item) IdManager.GetThingById(lastItemId.Value));
    }
}

[TaskDescription("Initializer for the HaulToZone job")]
[TaskCategory("Initializer")]
public class HaulItemToBlueprintInitializer : GrimAction
{
    public override void OnStart()
    {
        base.OnStart();

        CurrentJob.MakeReservations(Creature);
    }

    public override TaskStatus OnUpdate()
    {
        if (!ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}