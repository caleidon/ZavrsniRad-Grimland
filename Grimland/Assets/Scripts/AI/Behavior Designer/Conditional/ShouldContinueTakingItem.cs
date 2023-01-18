using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Determines if creature should keep looking for nearby items and if so, finds them and assigns them to the tree")]
public class ShouldContinueTakingItem : GrimConditional
{
    public SharedInt ImportZoneCapacity;
    public SharedVector3Int ImportExportLastItemNode;

    public SharedString ExportItemId;

    private Item itemInHand;

    public override void OnStart()
    {
        base.OnStart();
        itemInHand = Creature.ItemTracker.HeldItem;
    }

    public override TaskStatus OnUpdate()
    {
        if (!ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        var maxAmountInHand = itemInHand.MaxStackSize;

        if (itemInHand.Amount >= ImportZoneCapacity.Value || itemInHand.Amount >= maxAmountInHand)
        {
            return TaskStatus.Failure;
        }

        var floodedNodes = FloodFill.FloodAllTiles(ImportExportLastItemNode.Value, 4);

        foreach (var node in floodedNodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);
            if (nodeData.TryGetItem(out Item existingItem))
            {
                if (itemInHand.HasSameDefAs(existingItem))
                {
                    if (HaulableJobManager.ExistsHaulingJob(existingItem, out HaulToZoneJob haulingJob))
                    {
                        if (ReservationManager.TryReserve(Creature, existingItem, CurrentJob, ReservationType.HaulFrom))
                        {
                            ExportItemId.Value = existingItem.Id;
                            ImportExportLastItemNode.Value = existingItem.GetNode();
                            return TaskStatus.Success;
                        }
                    }
                }
            }
        }

        return TaskStatus.Failure;
    }
}