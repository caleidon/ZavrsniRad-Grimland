using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DetermineNextHaulDestination : GrimAction
{
    public SharedString ImportZoneId;
    public SharedHaulDestination ExportHaulDestination;
    public SharedVector3Int ExportDepositNode;

    private Zone currentZone;
    private Item itemInHand;

    public override void OnStart()
    {
        base.OnStart();

        currentZone = ZoneManager.Zones[ImportZoneId.Value];
        itemInHand = Creature.ItemTracker.HeldItem;
    }

    // TODO: this should be the main haul destination decider, we don't need to do it in the Initializer or job itself cuz this repeats anyway

    public override TaskStatus OnUpdate()
    {
        HaulDestination haulDestination = currentZone.GetHaulDestination(itemInHand);

        switch (haulDestination.Type)
        {
            case HaulDestination.DestinationType.None:
                return TaskStatus.Failure;

            case HaulDestination.DestinationType.Node:
                if (!HaulReservationManager.TryReserve(Creature, haulDestination.DestinationNode, CurrentJob))
                {
                    return TaskStatus.Failure;
                }

                break;

            case HaulDestination.DestinationType.Item:
                if (!ReservationManager.TryReserve(Creature, haulDestination.DestinationItem, CurrentJob, ReservationType.HaulTo))
                {
                    return TaskStatus.Failure;
                }

                if (!HaulReservationManager.TryReserve(Creature, haulDestination.DestinationNode, CurrentJob))
                {
                    return TaskStatus.Failure;
                }

                break;

            case HaulDestination.DestinationType.Inventory:
                return TaskStatus.Failure;

            default:
                throw new ArgumentOutOfRangeException();
        }

        ExportHaulDestination.Value = haulDestination;
        ExportDepositNode.Value = haulDestination.DestinationNode;

        return TaskStatus.Success;
    }
}