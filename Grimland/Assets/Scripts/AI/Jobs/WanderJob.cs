using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class WanderJob : Job
{
    public override bool Suspendable => false;
    public override bool AutoRestart => false;
    public override Creature.PawnMovementUrgency MovementUrgency => Creature.PawnMovementUrgency.Walk;

    // TODO: After getting to wander destination, don't wait a random amount if there is a job available
    public WanderJob(Creature creature)
    {
        if (Map.Instance.GetRandomRegionNode(creature.GetNode(), true, out Vector3Int randomPos))
        {
            KnownLocations.Add(randomPos);
        }
    }

    public override bool CanBeDoneBy(Creature creature)
    {
        return true;
    }
}

[TaskDescription("Initializer for the Wander job")]
[TaskCategory("Initializer")]
public class WanderInitializer : GrimAction
{
    public SharedVector3Int ExportWanderLocation;

    public override void OnStart()
    {
        base.OnStart();

        ExportWanderLocation.Value = CurrentJob.KnownLocations[0];
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