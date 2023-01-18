using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class DepositItem : GrimAction
{
    public SharedHaulDestination ImportHaulDestination;

    private Item itemInHand;
    private HaulDestination haulDestination;

    public override void OnStart()
    {
        base.OnStart();

        haulDestination = ImportHaulDestination.Value;
        itemInHand = Creature.ItemTracker.HeldItem;
    }

    public override TaskStatus OnUpdate()
    {
        switch (haulDestination.Type)
        {
            case HaulDestination.DestinationType.None:
                Debug.LogError("Tried to deposit item but DestinationType was None!");
                break;

            case HaulDestination.DestinationType.Node:
                Debug.Log($"[DepositItem] Item dropped: {Creature.ItemTracker.HeldItem} with amount {Creature.ItemTracker.HeldItem.Amount}");
                Creature.ItemTracker.DropItemOnFloor();
                break;

            case HaulDestination.DestinationType.Item:
                switch (haulDestination.DestinationItem.State)
                {
                    case Item.ItemState.None:
                        Debug.LogError("Tried to deposit item into another item but State was None;");
                        break;

                    case Item.ItemState.InHand:
                        Debug.Log($"[DepositItem] Item dropped: {Creature.ItemTracker.HeldItem} with amount {Creature.ItemTracker.HeldItem.Amount}");
                        Creature.ItemTracker.DropItemOnFloor();
                        break;

                    case Item.ItemState.InInventory:
                        Debug.Log($"[DepositItem] Item dropped: {Creature.ItemTracker.HeldItem} with amount {Creature.ItemTracker.HeldItem.Amount}");
                        Creature.ItemTracker.DropItemOnFloor();
                        break;

                    case Item.ItemState.InWorld:
                        int amountToGive = itemInHand.Amount > haulDestination.FreeSpace ? haulDestination.FreeSpace : itemInHand.Amount;
                        Creature.ItemTracker.TryCombineItem(haulDestination.DestinationItem, amountToGive);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;

            case HaulDestination.DestinationType.Inventory:
                Debug.LogError("Inventory not implemented!");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        HaulReservationManager.Release(Creature, haulDestination.DestinationNode, CurrentJob);
        ReservationManager.Release(Creature, haulDestination.DestinationItem, CurrentJob, ReservationType.HaulTo);

        return TaskStatus.Success;
    }
}