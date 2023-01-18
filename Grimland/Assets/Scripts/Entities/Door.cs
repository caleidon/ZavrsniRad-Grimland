// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Door : MonoBehaviour, IEntity
// {
//     public enum DoorState
//     {
//         Closed = 0,
//         Open = 1,
//         Opening = 2,
//         Closing = 3
//     }
//
//     public string ID { get; set; }
//     public Vector3Int Location { get; set; }
//     public bool IsSelectable { get; } = true;
//
//     public void Damage()
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Destroy()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Vector3 GetLocation()
//     {
//         return Map.GridPosFromWorldPos(transform.position);
//     }
//
//     public ISaver GetSaver()
//     {
//         throw new NotImplementedException();
//     }
//
//     public string DisplayName { get; set; }
//
//     public DoorState currentState;
//
//     public List<Sprite> sprites = new List<Sprite>();
//
//     public SpriteRenderer spriteRenderer;
//
//     public float TimeToOpen { get; set; }
//
//     public float openingTimer;
//
//     public Region DoorRegion;
//
//
//     public class Initializer
//     {
//         public string Name { get; set; }
//         public string DisplayName { get; set; }
//
//         public float TimeToOpen { get; set; }
//
//         public List<string> Sprites = new List<string>();
//
//         public List<Sprite> LoadedSprites = new List<Sprite>();
//
//         public void Initialize()
//         {
//             for (int i = 0; i < Sprites.Count; i++)
//             {
//                 // LoadedSprites.Add(DefManager.LoadSprite($"{Settings.DOORS_DEFS_PATH}/Sprites/{Name}/{Sprites[i]}"));
//             }
//         }
//
//         public GameObject Create(Vector3Int node)
//         {
//             GameObject doorGO = new GameObject("Door");
//             Door door = doorGO.AddComponent<Door>();
//             doorGO.transform.position = node;
//
//             door.TimeToOpen = TimeToOpen;
//             door.DisplayName = DisplayName;
//             door.sprites = LoadedSprites;
//             door.currentState = DoorState.Closed;
//
//             SpriteRenderer sr = doorGO.AddComponent<SpriteRenderer>();
//             sr.sortingOrder = 1;
//             sr.sprite = LoadedSprites[0];
//             door.spriteRenderer = sr;
//
//             // door.DoorRegion = Map.Instance.GetRegionFromNode(node);
//             return doorGO;
//         }
//     }
// }

