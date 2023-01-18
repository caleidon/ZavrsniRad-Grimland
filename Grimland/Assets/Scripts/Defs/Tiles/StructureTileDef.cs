// using System;
// using System.Collections.Generic;
// using JetBrains.Annotations;
// using UnityEngine;
//
// public class StructureTileDef : TileDef
// {
//     public Structure.StructureType[,] structure { get; set; }
//     public int MaxHP { get; set; }
//     public Dictionary<string, string> DirectionSprites = new Dictionary<string, string>();
//     public List<string> ApplicableJobs { get; set; }
//
//     [LoadedByYaml(YamlTag = "!StructureTile")]
//     public class Initializer : IInitializable
//     {
//         public string Name { get; set; }
//         public string DisplayName { get; set; }
//         public float MoveSpeedMultiplier { get; set; }
//
//         public List<string[]> YamlStructure { get; set; }
//
//         // TODO REMOVE DIRECTSPRITES INITIALIZATION
//         public Dictionary<string, string> DirectionSprites = new Dictionary<string, string>();
//         public List<string> ApplicableJobs { get; set; }
//
//
//         public object CreateInstance()
//         {
//             StructureTileDef structureTileDef = CreateInstance<StructureTileDef>();
//
//             structureTileDef.Name = Name;
//             structureTileDef.DisplayName = DisplayName;
//             structureTileDef.MoveSpeedMultiplier = MoveSpeedMultiplier;
//             structureTileDef.structure = Structure.ConvertYamlStructure(YamlStructure);
//             structureTileDef.DirectionSprites = DirectionSprites;
//             structureTileDef.ApplicableJobs = ApplicableJobs;
//
//             return structureTileDef;
//         }
//     }
// }
//
// public class StructureTile : Tile
// {
//     public string Id { get; set; }
//     public StructureTileDef StructureTileDef { get; }
//     public Vector3Int Location { get; set; }
//     public Dictionary<Vector3Int, Structure.StructureType> structure { get; set; }
//     public override bool IsWalkable => StructureTileDef.MoveSpeedMultiplier > 0;
//
//     public Type GetThingType()
//     {
//         return GetType();
//     }
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
//     public StructureTile([CanBeNull] string id, Vector3Int pivot, StructureTileDef structureTileDef, MatrixUtils.MatrixRotation rotation)
//     {
//         Id = IdManager.GenerateThingID(this, structureTileDef.Name, id);
//         StructureTileDef = structureTileDef;
//         Location = pivot;
//         structure = Structure.InitializeWorldStructure(pivot, structureTileDef.structure, rotation);
//     }
//
//     public ISaver GetSaveData()
//     {
//         throw new NotImplementedException();
//     }
// }

