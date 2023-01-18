// using BehaviorDesigner.Runtime;
// using BehaviorDesigner.Runtime.Tasks;
// using UnityEngine;
//
// [TaskDescription("Gets a random node within the current region and optionally, the neighboring regions.")]
// [TaskCategory("Informational")]
// public class GetRandomNode : Action
// {
//     public bool includeNeighborRegions = true;
//     public SharedVector3Int randomNode;
//
//     public override TaskStatus OnUpdate()
//     {
//         if (Map.Instance.GetRandomRegionNode(Utilities.V3ToV3I(transform.position), includeNeighborRegions, out Vector3Int randomPos))
//         {
//             randomNode.Value = randomPos;
//             return TaskStatus.Success;
//         }
//
//         return TaskStatus.Failure;
//     }
// }

