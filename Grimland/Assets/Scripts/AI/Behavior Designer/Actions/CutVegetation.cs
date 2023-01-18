using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Cuts down a specified vegetation")]
[TaskCategory("Action")]
public class CutVegetation : GrimAction
{
    public SharedString ImportVegetationId;
    private VegetationTile vegetation { get; set; }


    public override void OnStart()
    {
        vegetation = (VegetationTile) IdManager.GetThingById(ImportVegetationId.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (vegetation.IsDestroyed)
        {
            return TaskStatus.Success;
        }

        // TODO: This damage has to be acquired from axe, base creature damage, or research
        vegetation.Damage(10);
        return TaskStatus.Running;
    }
}