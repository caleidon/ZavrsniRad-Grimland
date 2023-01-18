using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Returns success if the amount of item the creature is holding in hand is equal or greater than the required amount")]
public class HasCreatureItemAmountInHand : GrimConditional
{
    public SharedInt ImportRequiredAmount;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (!Creature.ItemTracker.HasItemInHand)
        {
            return TaskStatus.Failure;
        }

        Item itemInHand = Creature.ItemTracker.HeldItem;

        return itemInHand.Amount >= ImportRequiredAmount.Value ? TaskStatus.Success : TaskStatus.Failure;
    }
}