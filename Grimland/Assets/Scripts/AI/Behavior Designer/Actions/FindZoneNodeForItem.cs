using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FindZoneNodeForItem : Action
{
    public SharedString ImportZoneId;

    public SharedVector3Int ExportDepositNode;

    private Zone zone;

    public override void OnStart()
    {
        zone = ZoneManager.Zones[ImportZoneId.Value];
    }

    public override TaskStatus OnUpdate()
    {
        if (!zone.TryGetEmptyUnreservedNode(out Vector3Int freeNode))
        {
            return TaskStatus.Failure;
        }

        ExportDepositNode.Value = freeNode;
        return TaskStatus.Success;
    }
}