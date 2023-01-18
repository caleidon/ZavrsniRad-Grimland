using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskDescription("Move the agent near a specific location within a given threshold.")]
[TaskCategory("Movement")]
public class MoveNear : MoveToNode
{
    private AIPath aiPath;
    public SharedVector3Int destination;

    public override void OnAwake()
    {
        aiPath = GetComponent<AIPath>();
    }

    public override void OnStart()
    {
        aiPath.destination = destination.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if (!aiPath.reachedDestination)
        {
            return TaskStatus.Running;
        }

        aiPath.destination = Vector3.positiveInfinity;
        return TaskStatus.Success;
    }

    public void OnJobComplete()
    {
        throw new NotImplementedException();
    }
}