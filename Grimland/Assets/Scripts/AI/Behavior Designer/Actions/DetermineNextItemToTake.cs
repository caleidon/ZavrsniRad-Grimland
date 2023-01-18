using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DetermineNextItemToTake : GrimAction
{
    public SharedString ExportItemToTakeId;
    public SharedVector3Int ExportItemToTakeNode;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (Creature.ItemTracker.HasItemInHand && Creature.ItemTracker.HeldItem.IsFull)
        {
            return TaskStatus.Failure;
        }

        if (((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.Count <= 0)
        {
            return TaskStatus.Failure;
        }

        Item nextItemToTake = ((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp[0];
        ((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.RemoveAt(0);

        ExportItemToTakeId.Value = nextItemToTake.Id;
        // TODO: here the item could be in an inventory, so we need GetInteractionNode so we can only walk up to the chest
        ExportItemToTakeNode.Value = nextItemToTake.GetNode();

        return TaskStatus.Success;
    }
}