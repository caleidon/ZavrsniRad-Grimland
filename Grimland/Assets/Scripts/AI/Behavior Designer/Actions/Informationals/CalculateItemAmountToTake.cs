using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Calculates the amount of an item the creature should take based on zone capacity, max item capacity and the currently held item.")]
public class CalculateItemAmountToTake : GrimAction
{
    public SharedInt ExportItemAmountToTake;

    public SharedInt ImportWantedAmount;
    public SharedString ImportItemToTakeId;

    private Item itemInHand;
    private Item itemToTake;

    public override void OnStart()
    {
        base.OnStart();
        itemInHand = Creature.ItemTracker.HeldItem;
        itemToTake = (Item) IdManager.GetThingById(ImportItemToTakeId.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (!ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        var wantedAmount = ImportWantedAmount.Value - itemInHand?.Amount ?? ImportWantedAmount.Value;

        var maxAmountInHand = itemToTake.MaxStackSize;
        var capacityLeft = maxAmountInHand - itemInHand?.Amount ?? maxAmountInHand;

        ExportItemAmountToTake.Value = itemToTake.Amount > wantedAmount ? wantedAmount : itemToTake.Amount;

        if (ExportItemAmountToTake.Value > capacityLeft)
        {
            ExportItemAmountToTake.Value = capacityLeft;
        }

        return TaskStatus.Success;
    }
}