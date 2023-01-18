using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskDescription("Move the creature to a specified Node")]
[TaskCategory("Movement")]
public class MoveToNode : GrimAction
{
    public SharedVector3Int ImportTargetNode;

    private AIPath aiPath;
    private bool pathPossible;

    public override void OnAwake()
    {
        aiPath = GetComponent<AIPath>();
    }

    private void TileUpdateDetected()
    {
        if (ReachabilityUtils.IsPathPossible(Creature.GetNode(), ImportTargetNode.Value))
        {
            aiPath.destination = ImportTargetNode.Value;
            aiPath.SearchPath();
        }
        else
        {
            pathPossible = false;
        }
    }

    public override void OnStart()
    {
        base.OnStart();

        // this.FailOn(() => true);

        Map.OnTileUpdate += TileUpdateDetected;

        pathPossible = true;
        aiPath.destination = ImportTargetNode.Value;
        aiPath.SearchPath();
    }

    public override TaskStatus OnUpdate()
    {
        if (!pathPossible || !ReservationsSuccessful || !LocalFailConditionsSatisfied || !GlobalFailAndEndConditionsSatisfied)
        {
            return TaskStatus.Failure;
        }

        if (!aiPath.reachedDestination)
        {
            return TaskStatus.Running;
        }

        // TODO: implement this in custom movement script eventually
        aiPath.SetPath(null);
        aiPath.destination = Vector3.positiveInfinity;
        return TaskStatus.Success;
    }

    public override void OnEnd()
    {
        Map.OnTileUpdate -= TileUpdateDetected;
    }
}