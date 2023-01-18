using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class HaulToZoneJob : Job
{
    public Item Item { get; }
    public Zone ExportZone { get; set; }
    public int ExportZoneCapacity { get; set; }
    public HaulDestination HaulDestination { get; set; }

    public HaulToZoneJob(Item item)
    {
        Item = item;
        KnownLocations.Add(item.GetNode());
    }

    public override bool Suspendable => false;
    public override bool AutoRestart => false;
    public override Creature.PawnMovementUrgency MovementUrgency => Creature.PawnMovementUrgency.Jog;

    public override bool CanBeDoneBy(Creature creature)
    {
        // TODO: make this virtual and check if creature can reach the known locations, then call base here
        if (creature.ItemTracker.HasItemInHand)
        {
            return creature.ItemTracker.HeldItem.GetRemainingSpace() > 0;
        }

        return true;
    }

    public override bool AreRequirementsFulfilled(Creature creature)
    {
        if (ZoneManager.Zones.Count <= 0)
        {
            return false;
        }

        if (!ZoneManager.ExistsZoneWithFreeSpace(Item))
        {
            return false;
        }

        return true;
    }

    public override bool MakeReservations(Creature creature)
    {
        if (!ReservationManager.TryReserve(creature, Item, this, ReservationType.HaulFrom))
        {
            return false;
        }

        if (!ZoneManager.TryFindZoneWithUnreservedSpaceForItem(Item, out Zone foundZone, out HaulDestination haulDestination, out int totalSpace))
        {
            return false;
        }

        switch (haulDestination.Type)
        {
            case HaulDestination.DestinationType.None:
                return false;

            case HaulDestination.DestinationType.Node:
                if (!HaulReservationManager.TryReserve(creature, haulDestination.DestinationNode, this))
                {
                    return false;
                }

                break;

            case HaulDestination.DestinationType.Item:
                if (!ReservationManager.TryReserve(creature, haulDestination.DestinationItem, this, ReservationType.HaulTo))
                {
                    return false;
                }
                if (!HaulReservationManager.TryReserve(creature, haulDestination.DestinationNode, this))
                {
                    return false;
                }

                break;

            case HaulDestination.DestinationType.Inventory:
                return false;

            default:
                throw new ArgumentOutOfRangeException();
        }

        HaulDestination = haulDestination;
        ExportZone = foundZone;
        ExportZoneCapacity = totalSpace;

        return true;
    }

    public override void OnJobFailed(Creature creature)
    {
        base.OnJobFailed(creature);

        if (creature.ItemTracker.HasItemInHand)
        {
            Debug.Log($"[HaulToZoneFailed] Item dropped: {creature.ItemTracker.HeldItem} with amount {creature.ItemTracker.HeldItem.Amount}");
            creature.ItemTracker.DropItemOnFloor();
        }

        BehaviorTree bt = creature.CreatureGO.GetComponent<BehaviorTree>();
        var lastItemId = (SharedString) bt.GetVariable("ItemId");
        HaulableJobManager.Notify_Spawned((Item) IdManager.GetThingById(lastItemId.Value));
    }
}

[TaskDescription("Initializer for the HaulToZone job")]
[TaskCategory("Initializer")]
public class HaulToZoneInitializer : GrimAction
{
    public SharedString ExportItemId;
    public SharedVector3Int ExportItemNode;
    public SharedString ExportZoneId;
    public SharedInt ExportZoneCapacity;
    public SharedHaulDestination ExportHaulDestination;
    public SharedVector3Int ExportDepositNode;

    public override void OnStart()
    {
        base.OnStart();

        CurrentJob.MakeReservations(Creature);

        ExportItemId.Value = ((HaulToZoneJob) CurrentJob).Item.Id;
        ExportItemNode.Value = ((HaulToZoneJob) CurrentJob).Item.GetNode();
        ExportZoneId.Value = ((HaulToZoneJob) CurrentJob).ExportZone.Id;
        ExportZoneCapacity.Value = ((HaulToZoneJob) CurrentJob).ExportZoneCapacity;
        ExportHaulDestination.Value = ((HaulToZoneJob) CurrentJob).HaulDestination;
        ExportDepositNode.Value = ((HaulToZoneJob) CurrentJob).HaulDestination.DestinationNode;
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