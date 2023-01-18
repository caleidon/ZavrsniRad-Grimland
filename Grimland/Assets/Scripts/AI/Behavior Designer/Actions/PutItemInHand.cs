using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Puts an amount of a certain item in this creature's hand")]
[TaskCategory("Items")]
public class PutItemInHand : GrimAction
{
    public SharedString ImportItemId;
    public SharedInt ImportItemAmountToTake;

    private Item itemToTake;

    public override void OnStart()
    {
        base.OnStart();

        itemToTake = (Item) IdManager.GetThingById(ImportItemId.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (!ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        if (Creature.ItemTracker.TryTakeItem(itemToTake, ImportItemAmountToTake.Value))
        {
            ReservationManager.Release(Creature, itemToTake, CurrentJob, ReservationType.HaulFrom);
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}