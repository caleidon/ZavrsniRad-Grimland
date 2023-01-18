using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DetermineNextBlueprint : GrimAction
{
    public SharedString ExportBlueprintTargetId;
    public SharedVector3Int ExportBlueprintNode;

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

        if (((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.Count <= 0)
        {
            return TaskStatus.Failure;
        }

        Blueprint nextBlueprint = ((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo[0];
        ((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.RemoveAt(0);

        ExportBlueprintTargetId.Value = nextBlueprint.Id;

        // TODO: here we need getinteraction node because we don't want to walk into the blueprint
        ExportBlueprintNode.Value = nextBlueprint.GetNode();
        return TaskStatus.Success;
    }
}