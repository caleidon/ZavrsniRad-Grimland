// using UnityEngine;
//
// public class ItemTileDef : TileDef
// {
//     public ItemDef ItemDef { get; private set; }
//
//     public static ItemTileDef Initialize(ItemDef itemDef)
//     {
//         ItemTileDef itemTileDef = CreateInstance<ItemTileDef>();
//         itemTileDef.ItemDef = itemDef;
//         itemTileDef.sprite = DefManager.AllSprites[itemDef.Sprite];
//
//         return itemTileDef;
//     }
//
//     public ItemTile Place(Vector3Int node, Item item, ItemTile existingItemTile, bool updateRegion = true)
//     {
//         ItemTile itemTile = existingItemTile ?? new ItemTile(null, item, node);
//
//         NodeManager.GetNodeDataAt(node).AddThing(itemTile);
//
//         Map.Instance.SetTile(node, this, Map.TilemapType.ItemTilemap, updateRegion: updateRegion);
//         return itemTile;
//     }
// }

// public class ItemTile : Tile, ICanBeSaved, ILocatable, IDamagable
// {
//     public string Id { get; set; }
//     public ItemTileDef ItemTileDef { get; set; }
//     public Inventory Inventory { get; set; }
//     public Vector3Int Node { get; }
//     public override bool IsWalkable => true;
//
//     public bool IsRegionThing => false;
//
//     // TODO: ItemTile should always be displaying health of the only item he's holding
//     public int Health { get; set; }
//
//     public event Action OnDamageTaken;
//     public event Action OnDestroy;
//
//
//     public ItemTile([CanBeNull] string id, Item item, Vector3Int node)
//     {
//         Id = IdManager.GenerateThingID(this, $"{item.GetItemDef().DisplayName}{GetType().Name}", id);
//         ItemTileDef = item.GetItemDef().ItemTileDef;
//         Node = node;
//
//         Inventory = new Inventory(null, 1, false) { Holder = this };
//         Inventory.AddEntireItem(item, true);
//
//         Inventory.OnEmptied += OnItemTileEmpty;
//
//         // TODO: Only do this if you're outside of a zone or in an incompatible zone
//         if (!ZoneManager.ZoneNodes.ContainsKey(Node))
//         {
//             JobManager.EnqueueJob(new HaulToZoneJob(item));
//         }
//     }
//
//     public ItemTile([CanBeNull] string id, Vector3Int node)
//     {
//         Id = IdManager.GenerateThingID(this, $"{GetType().Name}", id);
//         Node = node;
//     }
//
//
//     public Vector3Int GetNode()
//     {
//         return Node;
//     }
//
//     public void OnItemTileEmpty(object sender, Inventory.InventoryChangeEventArgs eventArgs)
//     {
//         // If this item gets picked up or destroyed, remove the hauling job if it exists
//         if (RegionManager.GetRegionFromNode(eventArgs.Item.GetNode(), out Region region))
//         {
//             if (JobManager.availableJobs.ContainsKey(region))
//             {
//                 // TODO: remove ToList() here, optimize
//                 foreach (var job in JobManager.availableJobs[region].ToList())
//                 {
//                     if (job is HaulToZoneJob haulToZoneJob)
//                     {
//                         if (haulToZoneJob.Item == eventArgs.Item)
//                         {
//                             JobManager.availableJobs[region].Remove(job);
//                         }
//                     }
//                 }
//             }
//         }
//
//         Destroy();
//     }
//
//     public void Damage(int damage)
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Destroy()
//     {
//         IdManager.RemoveFromThingList(Id);
//         IdManager.RemoveFromThingList(Inventory.Id);
//         NodeManager.GetNodeDataAt(Node).RemoveThing(this);
//
//         Map.Instance.SetTile(Node, null, Map.TilemapType.ItemTilemap);
//         OnDestroy?.Invoke();
//     }
//
//
//     public ISaver GetSaver()
//     {
//         return new ItemTileSaver(this);
//     }
// }
//
// public class ItemTileSaver : ISaver
// {
//     public string Id { get; set; }
//     public Vector3Int Node { get; set; }
//     public string InventoryId { get; set; }
//
//     public ItemTileSaver(ItemTile itemTile)
//     {
//         Id = itemTile.Id;
//         Node = itemTile.Node;
//         InventoryId = itemTile.Inventory.Id;
//     }
//
//     public ItemTileSaver() { }
//
//     private ItemTile itemTile;
//
//     public void Load(ISaver.Phase phase)
//     {
//         switch (phase)
//         {
//             case ISaver.Phase.Create:
//                 itemTile = new ItemTile(Id, Node);
//                 break;
//
//             case ISaver.Phase.Link:
//                 break;
//
//             case ISaver.Phase.Instantiate:
//                 Inventory inventory = (Inventory)IdManager.GetThingById(InventoryId);
//                 Item item = inventory.Items[0];
//
//                 itemTile.ItemTileDef = item.GetItemDef().ItemTileDef;
//                 itemTile.Inventory = inventory;
//                 itemTile.Inventory.OnEmptied += itemTile.OnItemTileEmpty;
//
//                 itemTile.ItemTileDef.Place(Node, item, itemTile, false);
//
//                 // TODO: Only do this if you're outside of a zone or in an incompatible zone
//                 if (!ZoneManager.ZoneNodes.ContainsKey(Node))
//                 {
//                     JobManager.EnqueueJob(new HaulToZoneJob(item));
//                 }
//
//                 break;
//         }
//     }
// }

