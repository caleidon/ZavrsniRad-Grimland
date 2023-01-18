// using BehaviorDesigner.Runtime;
// using BehaviorDesigner.Runtime.Tasks;
// using UnityEngine;
//
// public class PutHandItemOnFloor : Action
// {
//     public SharedVector3Int ImportItemDropNode;
//
//     private Item itemInHand;
//
//     public override void OnStart()
//     {
//         itemInHand = Owner.FindTask<JobChooser>().Creature.HandInventory.Items[0];
//     }
//
//     public override TaskStatus OnUpdate()
//     {
//         itemInHand.Inventory.RemoveItem(itemInHand);
//         itemInHand.GetItemDef().PlaceInWorld(ImportItemDropNode.Value, itemInHand);
//
//         return TaskStatus.Success;
//     }
// }

