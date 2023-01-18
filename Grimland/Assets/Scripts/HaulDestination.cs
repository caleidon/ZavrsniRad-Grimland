using UnityEngine;

public class HaulDestination
{
    public enum DestinationType
    {
        None,
        Node,
        Item,
        Inventory
    }

    public DestinationType Type { get; set; }
    public Item DestinationItem { get; set; }
    public int FreeSpace { get; set; }
    public Vector3Int DestinationNode { get; set; }
    public Inventory DestinationInventory { get; set; }

    public HaulDestination(Vector3Int node)
    {
        Type = DestinationType.Node;
        DestinationNode = node;
    }

    public HaulDestination(Item destinationItem, int freeSpace)
    {
        Type = DestinationType.Item;
        DestinationItem = destinationItem;
        DestinationNode = destinationItem.GetNode();
        FreeSpace = freeSpace;
    }

    public HaulDestination(Inventory inventory)
    {
        Type = DestinationType.Inventory;
        DestinationInventory = inventory;
        DestinationNode = inventory.GetNode();
    }

    public HaulDestination()
    {
        Type = DestinationType.None;
    }
}