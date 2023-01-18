using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DepositToBlueprint : GrimAction
{
    public SharedString ImportBlueprintId;

    private Blueprint blueprint;
    private Item itemInHand;

    public override void OnStart()
    {
        base.OnStart();
        blueprint = (Blueprint) IdManager.GetThingById(ImportBlueprintId.Value);
        itemInHand = Creature.ItemTracker.HeldItem;
    }

    public override TaskStatus OnUpdate()
    {
        int neededByBlueprint = blueprint.GetNextRequiredMaterialAmount(itemInHand.GetItemDef()).Count;
        int amountToDeposit = neededByBlueprint > itemInHand.Amount ? itemInHand.Amount : neededByBlueprint;

        Creature.ItemTracker.TryPutInInventory(blueprint.Inventory, amountToDeposit);

        ReservationManager.Release(Creature, blueprint, CurrentJob, ReservationType.HaulTo);
        return TaskStatus.Success;
    }
}