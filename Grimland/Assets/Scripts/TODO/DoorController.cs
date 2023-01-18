// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// public class DoorController : MonoBehaviour
// {
//     public static readonly HashSet<Door> doorsToUpdate = new HashSet<Door>();
//
//     private void Update()
//     {
//         foreach (var door in doorsToUpdate.ToList())
//         {
//             door.openingTimer += Time.deltaTime;
//
//             switch (door.currentState)
//             {
//                 case Door.DoorState.Closed:
//                     doorsToUpdate.Remove(door);
//                     break;
//
//                 case Door.DoorState.Open:
//
//                     if (door.DoorRegion.RegionThings.Count == 0)
//                     {
//                         if (door.openingTimer >= door.TimeToOpen)
//                         {
//                             door.currentState = Door.DoorState.Closing;
//                         }
//                     }
//                     else
//                     {
//                         door.openingTimer = 0;
//                     }
//
//                     break;
//
//                 case Door.DoorState.Opening:
//                     if (door.openingTimer >= door.TimeToOpen)
//                     {
//                         door.currentState = Door.DoorState.Open;
//                         door.spriteRenderer.sprite = door.sprites[1];
//
//                         door.openingTimer = 0;
//                     }
//
//                     break;
//
//                 case Door.DoorState.Closing:
//                     if (door.openingTimer >= door.TimeToOpen)
//                     {
//                         door.currentState = Door.DoorState.Closed;
//                         door.spriteRenderer.sprite = door.sprites[0];
//
//                         door.openingTimer = 0;
//                     }
//
//                     break;
//
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//         }
//     }
// }

