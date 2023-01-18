// using UnityEngine;
//
// public abstract class Door : MonoBehaviour
// {
//     public string Name { get; set; }
//
//
//     public string DisplayName { get; set; }
//     public float MoveSpeedMultiplier { get; set; }
//     public int MaxStackSize { get; set; }
//     public int MaxHP { get; set; }
//     public string Sprite { get; set; }
//     public bool IsStackable => MaxStackSize > 1;
//     public ItemTileDef ItemTileDef { get; set; }
//
//     [LoadedByYaml]
//     public abstract class BaseInitializer : IYamlInterface
//     {
//         public string Name { get; set; }
//         public string DisplayName { get; set; }
//         public float MoveSpeedMultiplier { get; set; }
//         public int MaxStackSize { get; set; }
//         public int MaxHP { get; set; }
//         public string Sprite { get; set; }
//
//         protected void InitBaseValues<T>(ref T original) where T : ItemDef
//         {
//             original.Name = Name;
//             original.DisplayName = DisplayName;
//             original.MoveSpeedMultiplier = MoveSpeedMultiplier;
//             original.MaxStackSize = MaxStackSize;
//             original.MaxHP = MaxHP;
//             original.Sprite = Sprite;
//             original.ItemTileDef = ItemTileDef.Initialize(original);
//         }
//
//         public abstract object CreateInstance();
//     }
//
//     public ItemTile Place(Vector3Int node, IItem item)
//     {
//         return ItemTileDef.Place(node, item, ItemTileDef);
//     }
//
//     public abstract IItem Create(int amount);
// }

