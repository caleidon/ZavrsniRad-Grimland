using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CutVegetationJob : Job
{
    public VegetationTile Vegetation { get; set; }
    public override bool Suspendable => false;
    public override bool AutoRestart => false;
    public override Creature.PawnMovementUrgency MovementUrgency => Creature.PawnMovementUrgency.Jog;

    public CutVegetationJob(VegetationTile vegetation)
    {
        Vegetation = vegetation;
        KnownLocations.Add(vegetation.GetNode());
    }

    public override bool CanBeDoneBy(Creature creature)
    {
        return true;
    }

    //TODO: need to add some sort of DeleteSelf() method that removes designation jobs when stuff is destroyed

    public override bool AreRequirementsFulfilled(Creature creature)
    {
        return true;
    }

    public override bool MakeReservations(Creature creature)
    {
        if (!ReservationManager.TryReserve(creature, Vegetation, this, ReservationType.Interact))
        {
            return false;
        }

        return true;
    }
}

[TaskDescription("Initializes for the CutVegetation job")]
[TaskCategory("Initializer")]
public class CutVegetationInitializer : GrimAction
{
    public SharedString ExportVegetationId;
    public SharedVector3Int ExportVegetationNode;

    // TODO: for this and in general, if trees are only interactable from main sides, and if pathing blocks main sides, we need to calculate new side and check pathfinding
    // this likely needs to be done in MoveTo where it has an option of what nodes to consider pathfindable before failing

    public override void OnStart()
    {
        base.OnStart();

        CurrentJob.MakeReservations(Creature);

        ExportVegetationId.Value = ((CutVegetationJob) CurrentJob).Vegetation.Id;
        ExportVegetationNode.Value = ((CutVegetationJob) CurrentJob).Vegetation.GetNode();
    }
}