using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Item : Thing, ICanBeSaved
{
    public enum ItemState
    {
        None,
        InHand,
        InInventory,
        InWorld
    }

    public enum CombinationResult
    {
        Failed,
        Partial,
        Full
    }

    public override bool IsRegionThing => true;
    public override ThingInteractionNode InteractionNode => ThingInteractionNode.Center;
    public Inventory Inventory { get; set; }
    public Creature Owner { get; set; }
    public Vector3Int Node { get; set; }
    public int Health { get; set; }
    public ItemState State { get; private set; }
    private int amount;

    public int MaxStackSize => ((ItemDef) ThingDef).MaxStackSize;
    public bool IsFull => Amount >= MaxStackSize;

    public int Amount
    {
        get => amount;
        set
        {
            amount = value;

            if (amount <= 0)
            {
                IdManager.RemoveFromThingList(Id);
                HaulableJobManager.Notify_Removed(this);
            }
        }
    }


    protected Item([CanBeNull] string id, ThingDef thingDef, int amount, ItemState state)
    {
        ThingDef = thingDef;
        Amount = amount;
        State = state;

        GenerateThingId(id);
    }


    public override Vector3Int GetNode()
    {
        switch (State)
        {
            case ItemState.None:
                Debug.LogError("Tried getting item's location and it wasn't initialized!");
                break;
            case ItemState.InHand:
                return Owner.GetNode();
            case ItemState.InInventory:
                return Inventory.GetNode();
            case ItemState.InWorld:
                return Node;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Node;
    }

    public ItemDef GetItemDef()
    {
        return ThingDef as ItemDef;
    }

    public abstract Item MakeCopy();

    public bool HasSameDefAs(Item item)
    {
        return GetItemDef().Name == item.GetItemDef().Name;
    }

    public int GetRemainingSpace()
    {
        return GetItemDef().MaxStackSize - Amount;
    }

    private bool CanCombineWith(Item itemToCombineWith, int amountToGive)
    {
        return HasSameDefAs(itemToCombineWith) && amountToGive + itemToCombineWith.Amount <= MaxStackSize;
    }

    public void TryCombineWith(Item itemToCombineWith, int amountToGive, out CombinationResult result)
    {
        if (!CanCombineWith(itemToCombineWith, amountToGive))
        {
            result = CombinationResult.Failed;
        }

        TransferHealthTo(itemToCombineWith);
        itemToCombineWith.Amount += amountToGive;
        Amount -= amountToGive;

        // Debug.Log($"Complete. I now have {Amount} and they have {itemToCombineWith.Amount}");

        result = Amount <= 0 ? CombinationResult.Full : CombinationResult.Partial;
    }

    public Item TakeAmount(int amountToTake)
    {
        if (amountToTake >= Amount)
        {
            Debug.LogError("Tried to take amount from item which would leave it empty!");
        }

        Item takenItem = MakeCopy();
        takenItem.Amount = amountToTake;
        Amount -= amountToTake;

        return takenItem;
    }

    private void TransferHealthTo(Item otherItem)
    {
        // TODO: this is probably broken
        int myHealth = Health * Amount;
        int theirHealth = otherItem.Health * otherItem.Amount;
        int combinedHealth = (myHealth + theirHealth) / (Amount + otherItem.Amount);

        otherItem.Health = combinedHealth;
    }

    public override void Destroy(DestroyMode destroyMode)
    {
        base.Destroy(destroyMode);

        HaulableJobManager.Notify_Removed(this);
    }

    public void PlaceInWorld(Vector3Int node)
    {
        State = ItemState.InWorld;
        Node = node;
        Owner = null;
        Inventory = null;
    }

    public void PlaceInInventory(Inventory inventory)
    {
        State = ItemState.InInventory;
        Inventory = inventory;
        Owner = null;
    }

    public void PlaceInHand(Creature newOwner)
    {
        State = ItemState.InHand;
        Owner = newOwner;
        Inventory = null;
    }

    public abstract ISaver GetSaver();
}