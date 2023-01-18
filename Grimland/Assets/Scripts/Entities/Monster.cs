// using UnityEngine;
//
// public class Monster : Creature
// {
//     public new class Initializer : Creature.Initializer
//     {
//         public void Initialize()
//         {
//             for (int i = 0; i < Sprites.Count; i++)
//             {
//                 // LoadedSprites.Add(DefManager.LoadSprite($"{Settings.CREATURES_MONSTERS_DEFS_PATH}/Sprites/{Name}/{Sprites[i]}"));
//             }
//         }
//
//         public override GameObject Create(Vector3Int spawnPosition)
//         {
//             GameObject monsterParent = base.Create(spawnPosition);
//             monsterParent.name = "Skelly";
//
//             Monster monster = CreatureGO.AddComponent<Monster>();
//             monster.DisplayName = DisplayName;
//             monster.loadedSprites = LoadedSprites;
//             monster.spriteRenderer = Sr;
//
//             return monsterParent;
//         }
//     }
// }

