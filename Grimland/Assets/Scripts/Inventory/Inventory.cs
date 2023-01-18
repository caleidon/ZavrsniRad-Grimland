using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public enum InventoryTransaction
    {
        ItemFullyAdded,
        ItemPartiallyAdded,
        Failure
    }

    public Thing Owner { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public readonly int maxSlots;
    private int FreeSlots => maxSlots - Items.Count;
    public readonly bool isPrivate;

    public event Action ItemAdded;

    public Inventory(int maxSlots, Thing owner, bool isPrivate)
    {
        this.maxSlots = maxSlots;
        Owner = owner;
        this.isPrivate = isPrivate;
    }

    public Vector3Int GetNode()
    {
        return Owner.GetNode();
    }


    public int GetContainedAmountOfItemType(ItemDef itemDef)
    {
        if (!ExistsItemOfType(itemDef))
        {
            return 0;
        }

        int totalAmountContained = 0;

        foreach (var item in Items)
        {
            if (item.GetItemDef().IsSameAs(itemDef))
            {
                totalAmountContained += item.Amount;
            }
        }

        return totalAmountContained;
    }

    private bool CanAddItem(ItemDef itemDef, int amountToAdd)
    {
        int availableSpace = GetAvailableSpaceForItemType(itemDef);
        return availableSpace >= amountToAdd;
    }

    public int GetAvailableSpaceForItemType(ItemDef itemDef)
    {
        int availableSpace = 0;

        for (int i = 0; i < FreeSlots; i++)
        {
            availableSpace += itemDef.MaxStackSize;
        }

        foreach (var existingItem in Items)
        {
            if (existingItem.GetItemDef().IsSameAs(itemDef))
            {
                availableSpace += existingItem.GetRemainingSpace();
            }
        }

        return availableSpace;
    }

    // private bool CanRemoveItem(Item item, int amountToRemove)
    // {
    //     int existingAmount = Items.Where(existingItem => existingItem.HasSameDefAs(item)).Sum(existingItem => existingItem.Amount);
    //
    //     return existingAmount >= amountToRemove;
    // }

    private bool FindIncompleteItemOfType(ItemDef itemDef, out Item foundItem, out int spaceLeft)
    {
        foreach (var otherItem in Items)
        {
            if (!otherItem.GetItemDef().IsSameAs(itemDef) || otherItem.IsFull)
            {
                continue;
            }

            foundItem = otherItem;
            spaceLeft = foundItem.GetRemainingSpace();
            return true;
        }

        spaceLeft = 0;
        foundItem = null;
        return false;
    }

    public InventoryTransaction AddItemAmount(Item item, int amountToAdd, bool addToRegionThings)
    {
        if (!CanAddItem(item.GetItemDef(), amountToAdd)) return InventoryTransaction.Failure;

        if (item.Amount == amountToAdd)
        {
            return AddItem(item, addToRegionThings);
        }

        Item partialItem = item.MakeCopy();
        partialItem.Amount = amountToAdd;
        item.Amount -= amountToAdd;

        AddItem(partialItem, addToRegionThings);
        return InventoryTransaction.ItemPartiallyAdded;
    }

    private InventoryTransaction AddItem(Item item, bool addToRegionThings)
    {
        bool itemAdded = false;
        item.PlaceInInventory(this);
        while (item.Amount > 0 && !itemAdded)
        {
            if (FindIncompleteItemOfType(item.GetItemDef(), out Item foundItem, out int spaceLeft))
            {
                int transferValue = item.Amount <= spaceLeft ? item.Amount : spaceLeft;

                // TODO: we aren't transferring health
                // item.TransferHealthTo(foundItem);

                foundItem.Amount += transferValue;
                item.Amount -= transferValue;
            }
            else
            {
                Items.Add(item);

                if (!isPrivate && addToRegionThings)
                {
                    RegionManager.AddToRegionThings(item, GetNode());
                }

                itemAdded = true;
            }
        }

        ItemAdded?.Invoke();
        return InventoryTransaction.ItemFullyAdded;
    }

    // public Item RemoveItemAmount(Item item, int amountToTake)
    // {
    //     if (!CanRemoveItem(item, amountToTake))
    //     {
    //         Debug.LogError("Tried to remove amount from item that wasn't available in the inventory!");
    //         return null;
    //     }
    //
    //     if (amountToTake > item.GetItemDef().MaxStackSize)
    //     {
    //         Debug.LogError("Tried to remove amount from item that is greater than it's MaxStackSize!");
    //         return null;
    //     }
    //
    //     Item exactItem = Items.Find(inventoryItem => inventoryItem.Amount == amountToTake);
    //     if (exactItem != null)
    //     {
    //         return RemoveItem(exactItem);
    //     }
    //
    //     Item requestedPartialItem = item.MakeCopy();
    //     requestedPartialItem.Amount = 0;
    //
    //     while (amountToTake > 0)
    //     {
    //         if (FindIncompleteItemOfType(item, out Item foundItem, out int spaceLeft))
    //         {
    //             // We already checked if an item with the exact amount exists above
    //             if (foundItem.Amount > amountToTake)
    //             {
    //                 // foundItem.TransferHealthTo(requestedPartialItem);
    //                 foundItem.Amount -= amountToTake;
    //                 requestedPartialItem.Amount = amountToTake;
    //                 return requestedPartialItem;
    //             }
    //
    //             // Here, the found item has less amount than needed
    //             // foundItem.TransferHealthTo(requestedPartialItem);
    //             int transferAmount = foundItem.Amount;
    //             foundItem.Amount -= transferAmount;
    //             requestedPartialItem.Amount += transferAmount;
    //             amountToTake -= transferAmount;
    //             RemoveItem(foundItem);
    //         }
    //         else
    //         {
    //             // if (!FindItemOfType(item, out Item fullStackItem))
    //             // {
    //             //     continue;
    //             // }
    //
    //             // fullStackItem.TransferHealthTo(requestedPartialItem);
    //             // fullStackItem.Amount -= amountToTake;
    //             requestedPartialItem.Amount += amountToTake;
    //             return requestedPartialItem;
    //         }
    //     }
    //
    //     return requestedPartialItem;
    // }

    // public Item RemoveItem(Item item)
    // {
    //     if (Items.Contains(item))
    //     {
    //         Items.Remove(item);
    //
    //         if (!isPrivate)
    //         {
    //             RegionManager.RemoveFromRegionThings(item, GetNode());
    //         }
    //
    //         return item;
    //     }
    //
    //     Debug.LogError("Inventory didn't contain item that was supposed to be removed!");
    //     return null;
    // }

    // private bool FindItemOfType(ItemDef itemDef, out Item foundItem)
    // {
    //     foreach (var otherItem in Items.Where(otherItem => otherItem.GetItemDef().IsSameAs(itemDef)))
    //     {
    //         foundItem = otherItem;
    //         return true;
    //     }
    //
    //     foundItem = null;
    //     return false;
    // }

    private bool ExistsItemOfType(ItemDef itemDef)
    {
        foreach (var item in Items)
        {
            if (item.GetItemDef().IsSameAs(itemDef))
            {
                return true;
            }
        }

        return false;
    }
    //
    // public ItemTile DropItemOnFloor(Item item)
    // {
    //     return null;
    // }

    // protected void DamageAllItems()
    // {
    //     foreach (var item in Items)
    //     {
    //         item.Damage();
    //     }
    // }

    // public override Vector3Int GetNode()
    // {
    //     return Holder.GetNode();
    // }


    // public ISaver GetSaver()
    // {
    //     return new InventorySaver(this);
    // }
}
//
// public class InventorySaver : ISaver
// {
//     public string Id { get; set; }
//     public string HolderId { get; set; }
//     public int MaxSlots { get; set; }
//     public bool IsPrivate { get; set; }
//
//     public InventorySaver(Inventory inventory)
//     {
//         Id = inventory.Id;
//         HolderId = inventory.Holder.Id;
//         MaxSlots = inventory.maxSlots;
//         IsPrivate = inventory.isPrivate;
//     }
//
//     public InventorySaver() { }
//
//     private Inventory inventory;
//
//     public void Load(ISaver.Phase phase)
//     {
//         switch (phase)
//         {
//             case ISaver.Phase.Create:
//                 inventory = new Inventory(Id, MaxSlots, IsPrivate);
//                 break;
//             case ISaver.Phase.Link:
//                 Thing holder = IdManager.GetThingById(HolderId);
//                 inventory.Holder = holder;
//                 break;
//             case ISaver.Phase.Instantiate:
//                 break;
//         }
//     }
// }