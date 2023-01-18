 using System;
using UnityEngine;

public class CreatureItemTracker
{
    public Creature Owner { get; set; }
    public Item HeldItem { get; set; }
    public bool HasItemInHand => HeldItem != null;

    public CreatureItemTracker(Creature owner)
    {
        Owner = owner;
        HeldItem = null;
    }

    public Inventory.InventoryTransaction TryPutInInventory(Inventory inventory, int amountToDeposit)
    {
        Inventory.InventoryTransaction result = inventory.AddItemAmount(HeldItem, amountToDeposit, true);

        switch (result)
        {
            case Inventory.InventoryTransaction.ItemFullyAdded:
                HeldItem = null;
                break;

            case Inventory.InventoryTransaction.ItemPartiallyAdded:
                break;

            case Inventory.InventoryTransaction.Failure:
                Debug.LogError("Tried putting item in hand to inventory but resulted in Failure!");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    public bool TryCombineItem(Item itemToCombine, int amountToGive)
    {
        if (!HasItemInHand)
        {
            Debug.LogError("Tried to combine item from hand but had none!");
            return false;
        }

        if (!ReservationManager.CanReserve(itemToCombine, ReservationType.HaulTo, out Creature owner))
        {
            if (owner.Id != Owner.Id)
            {
                Debug.LogError($"Trying to combine with item {itemToCombine.Id} I can't reserve! Already reserved by {owner.Id}");
            }
        }

        switch (itemToCombine.State)
        {
            case Item.ItemState.None:
                Debug.LogError("Tried taking item in hand that wasn't initialized!");
                return false;

            case Item.ItemState.InHand:
                Debug.LogError("Tried combining with item that was in someone else's hand!");
                return false;

            case Item.ItemState.InInventory:
                // TODO: implement inventory combining
                return false;

            case Item.ItemState.InWorld:
                HeldItem.TryCombineWith(itemToCombine, amountToGive, out Item.CombinationResult result);

                switch (result)
                {
                    case Item.CombinationResult.Failed:
                        Debug.LogError("Tried to merge item in hand with another but combination failed!");
                        return false;

                    case Item.CombinationResult.Partial:
                        Debug.Log($"Combined with another item partially. New amount is now: {HeldItem.Amount}");
                        return true;

                    case Item.CombinationResult.Full:
                        Debug.Log($"Combined fully. Other item's amount is now: {itemToCombine.Amount}");
                        HeldItem = null;
                        return true;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool TryTakeItem(Item itemToTake, int amountToTake)
    {
        if (!ReservationManager.CanReserve(itemToTake, ReservationType.HaulFrom, out Creature owner))
        {
            if (owner.Id != Owner.Id)
            {
                Debug.LogError($"Trying to take item {itemToTake.Id} I can't reserve! Already reserved by {owner.Id}");
            }
        }

        //TODO: check release reservations, maybe someone releases too early or something

        if (HasItemInHand)
        {
            if (itemToTake.HasSameDefAs(HeldItem))
            {
                switch (itemToTake.State)
                {
                    case Item.ItemState.None:
                        Debug.LogError("Tried taking item in hand that wasn't initialized!");
                        return false;

                    case Item.ItemState.InHand:
                        Debug.LogError("Tried taking item in hand from someone else's hand!");
                        return false;

                    case Item.ItemState.InInventory:
                        // TODO: implement inventory combining
                        return false;
                    case Item.ItemState.InWorld:

                        itemToTake.TryCombineWith(HeldItem, amountToTake, out Item.CombinationResult result);

                        switch (result)
                        {
                            case Item.CombinationResult.Failed:
                                Debug.LogError("Tried to take item in hand but combination failed!");
                                return false;

                            case Item.CombinationResult.Partial:
                                Debug.Log($"Picked up another item partially. New amount is now: {HeldItem.Amount}");
                                return true;

                            case Item.CombinationResult.Full:
                                NodeManager.GetNodeDataAt(itemToTake.Node).RemoveThing(itemToTake);
                                RegionManager.RemoveFromRegionThings(itemToTake, itemToTake.Node);
                                HaulableJobManager.Notify_Removed(itemToTake);
                                Map.Instance.SetTile(itemToTake.Node, null, Map.TilemapType.ItemTilemap, true);
                                Debug.Log($"Combined fully. New amount is now: {HeldItem.Amount}");
                                return true;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Debug.LogError("Tried taking item that wasn't the same Def as one in hand!");
            return false;
        }

        switch (itemToTake.State)
        {
            case Item.ItemState.None:
                Debug.LogError("Tried taking item in hand that wasn't initialized!");
                return false;

            case Item.ItemState.InHand:
                Debug.LogError("Tried taking item in hand from someone else's hand!");
                return false;

            case Item.ItemState.InInventory:
                // TODO: implement inventory combining
                return false;

            case Item.ItemState.InWorld:

                if (amountToTake > itemToTake.Amount)
                {
                    Debug.LogError("Tried to take in hand greater amount than item had!");
                    return false;
                }

                if (amountToTake < itemToTake.Amount)
                {
                    HeldItem = itemToTake.TakeAmount(amountToTake);
                    HeldItem.PlaceInHand(Owner);
                    Debug.Log($"Picked up a portion of an item. We now have {HeldItem} with amount {HeldItem.Amount}");
                    return true;
                }

                itemToTake.PlaceInHand(Owner);

                NodeManager.GetNodeDataAt(itemToTake.Node).RemoveThing(itemToTake);
                RegionManager.RemoveFromRegionThings(itemToTake, itemToTake.Node);
                HaulableJobManager.Notify_Removed(itemToTake);
                Map.Instance.SetTile(itemToTake.Node, null, Map.TilemapType.ItemTilemap, true);
                HeldItem = itemToTake;

                Debug.Log($"Picked up whole item: {HeldItem} with amount {HeldItem.Amount}");
                return true;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void DropItemOnFloor()
    {
        if (!HasItemInHand)
        {
            Debug.LogError("Tried to drop item in hand on floor and it didn't exist!");
            return;
        }

        if (HeldItem.State != Item.ItemState.InHand)
        {
            Debug.LogError("Tried to drop item in hand on floor and it wasn't in hand!");
            return;
        }

        HeldItem.PlaceInWorld(FindFreeDropLocation(Owner.GetNode()));

        NodeManager.GetNodeDataAt(HeldItem.Node).AddThing(HeldItem);
        RegionManager.AddToRegionThings(HeldItem, HeldItem.Node);
        HaulableJobManager.Notify_Spawned(HeldItem);

        Map.Instance.SetTile(HeldItem.Node, HeldItem.GetItemDef().Tile, Map.TilemapType.ItemTilemap, true);

        HeldItem = null;
    }

    // TODO: this NEEDS to be moved to Item or ItemUtility
    public static Vector3Int FindFreeDropLocation(Vector3Int center)
    {
        foreach (var node in RadialPatternGen.NodesInRadius(center, 1000, true))
        {
            // TODO: out of bounds possible here
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            if (!nodeData.TryGetItem(out Item existingItem))
            {
                return nodeData.Node;
            }
        }

        Debug.LogError("Couldn't find free drop location for item!");
        return Vector3Int.zero;
    }
}