using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Returns success if a zone with any available space was found for the item")]
public class FindZoneWithSpace : GrimConditional
{
    // TODO: subscribe to zone - if at any point it is deleted OR edited, cancel or retry this entire task
    // TODO: make sure that zone you DO FIND is actually compatible with this item via tags

    public SharedInt ExportZoneCapacity;
    public SharedString ExportZoneId;

    public SharedString ImportItemId;

    private Item item;

    // protected override bool TryMakeLocalReservations()
    // {
    //     if (ZoneManager.FindZoneWithAvailableSpaceForItem(item, out Zone foundZone, out int availableSpace))
    //     {
    //         switch (foundZone.GetHaulDestination(item, out Vector3Int node, out Item existingItem))
    //         {
    //             case Zone.HaulDestination.Error:
    //                 Debug.LogError($"Zone returned HaulDestination.Error!");
    //                 break;
    //     
    //             case Zone.HaulDestination.Node:
    //                 if (!HaulReservationManager.TryReserve(Creature, node, CurrentJob))
    //                 {
    //                     return false;
    //                 }
    //     
    //                 break;
    //     
    //             case Zone.HaulDestination.Inventory:
    //                 if (!ReservationManager.TryReserve(Creature, existingItem, CurrentJob, ReservationType.HaulFrom))
    //                 {
    //                     return false;
    //                 }
    //     
    //                 break;
    //     
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     
    //         ExportZoneId.Value = foundZone.Id;
    //         ExportZoneCapacity.Value = availableSpace;
    //     }
    //
    //     return true;
    // }

    public override void OnStart()
    {
        item = (Item)IdManager.GetThingById(ImportItemId.Value);
        base.OnStart();
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