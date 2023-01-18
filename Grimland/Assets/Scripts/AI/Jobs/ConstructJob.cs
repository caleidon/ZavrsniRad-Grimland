using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ConstructJob : Job
{
    public Blueprint Blueprint { get; set; }

    public ConstructJob(Blueprint blueprint)
    {
        Blueprint = blueprint;
        KnownLocations.Add(blueprint.GetNode());
    }

    public override bool Suspendable => false;
    public override bool AutoRestart => false;
    public override Creature.PawnMovementUrgency MovementUrgency => Creature.PawnMovementUrgency.Jog;

    public override bool CanBeDoneBy(Creature creature)
    {
        return true;
    }

    public override bool AreRequirementsFulfilled(Creature creature)
    {
        return true;
    }

    public override bool MakeReservations(Creature creature)
    {
        if (!ReservationManager.TryReserve(creature, Blueprint, this, ReservationType.Construct))
        {
            return false;
        }

        return true;
    }
}


public class ConstructInitializer : GrimAction
{
    public SharedString ExportBlueprintId;
    public SharedVector3Int ExportBlueprintNode;

    public override void OnStart()
    {
        base.OnStart();

        CurrentJob.MakeReservations(Creature);

        ExportBlueprintId.Value = ((ConstructJob) CurrentJob).Blueprint.Id;
        ExportBlueprintNode.Value = ((ConstructJob) CurrentJob).Blueprint.GetNode();
    }

    public override TaskStatus OnUpdate()
    {
        if (!ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}