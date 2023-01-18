// using System;
// using TMPro;
// using UnityEngine;
//
// public class ItemWorld : MonoBehaviour, IEntity
// {
//     private bool attachedToEntity;
//
//     private ItemDef itemDef;
//     [SerializeField] private SpriteRenderer spriteRenderer;
//     [SerializeField] private TextMeshPro textMeshPro;
//
//     private void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         textMeshPro = transform.Find("ItemAmountText").GetComponent<TextMeshPro>();
//     }
//
//     public static ItemWorld SpawnItemWorld(Vector3Int node, ItemDef itemDef)
//     {
//         Transform transform = Instantiate(PrefabManager.Instance.itemWorldPrefab, node, Quaternion.identity);
//
//         ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
//         itemWorld.SetItem(itemDef);
//
//         itemWorld.AddSelfToRegion();
//         itemWorld.attachedToEntity = false;
//
//         return itemWorld;
//     }
//
//     public void AttachToEntity(IEntity entity)
//     {
//         RemoveSelfFromRegion();
//         attachedToEntity = true;
//     }
//
//     public void DetachFromEntity()
//     {
//         AddSelfToRegion();
//         attachedToEntity = true;
//     }
//
//     public void AddSelfToRegion()
//     {
//         Vector3Int currentNode = Map.GridPosFromWorldPos(transform.position);
//         Region currentRegion = Map.Instance.GetRegionFromNode(currentNode);
//         currentRegion.ListerThings.Add(this);
//     }
//
//     public void RemoveSelfFromRegion()
//     {
//         Vector3Int currentNode = Map.GridPosFromWorldPos(transform.position);
//         Region currentRegion = Map.Instance.GetRegionFromNode(currentNode);
//         currentRegion.ListerThings.Remove(this);
//     }
//
//     public void SetItem(ItemDef newItemDef)
//     {
//         itemDef = newItemDef;
//         spriteRenderer.sprite = newItemDef.sprite;
//         textMeshPro.text = newItemDef.Amount > 1 ? newItemDef.Amount.ToString() : "";
//     }
//
//     public ItemDef GetItem()
//     {
//         return itemDef;
//     }
//
//     public Vector3Int Location { get; set; }
//
//     public void Damage()
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Destroy()
//     {
//         Destroy(gameObject);
//     }
//
//     public Vector3Int GetLocation()
//     {
//         return Map.GridPosFromWorldPos(transform.position);
//     }
//
//     public ISaver GetSaver()
//     {
//         throw new NotImplementedException();
//     }
//
//     public string ID { get; set; }
//     public bool IsSelectable { get; }
// }

