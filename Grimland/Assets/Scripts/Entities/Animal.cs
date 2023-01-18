// using UnityEngine;
//
// public class Animal : Creature
// {
//     public new class Initializer : Creature.Initializer
//     {
//         public void Initialize()
//         {
//             for (int i = 0; i < Sprites.Count; i++)
//             {
//                 // LoadedSprites.Add(DefManager.LoadSprite($"{Settings.CREATURES_ANIMALS_DEFS_PATH}/Sprites/{Name}/{Sprites[i]}"));
//             }
//         }
//
//         public override GameObject Create(Vector3Int spawnPosition)
//         {
//             GameObject animalParent = base.Create(spawnPosition);
//             animalParent.name = "Becky";
//
//             Animal animal = CreatureGO.AddComponent<Animal>();
//             animal.DisplayName = DisplayName;
//             animal.loadedSprites = LoadedSprites;
//             animal.spriteRenderer = Sr;
//
//             return animalParent;
//         }
//     }
// }

