using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Construct : GrimAction
{
    public SharedString BlueprintId;

    private Blueprint blueprint;

    public override void OnStart()
    {
        base.OnStart();
        blueprint = (Blueprint) IdManager.GetThingById(BlueprintId.Value);
    }

    public override TaskStatus OnUpdate()
    {
        blueprint.CurrentConstructionTicks++;

        if (blueprint.CurrentConstructionTicks >= blueprint.TicksToConstruct)
        {
            blueprint.FinishConstruction();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}