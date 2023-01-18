using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Blueprint : Tile
{
    public List<ItemDefCount> RequiredMaterials { get; }
    public int TicksToConstruct { get; set; }
    public override bool IsRegionThing => true;
    public Inventory Inventory { get; set; }
    public int CurrentConstructionTicks { get; set; }

    public Blueprint([CanBeNull] string id, Vector3Int node, ThingDef solidThingDef, List<ItemDefCount> requiredMaterials, int ticksToConstruct) : base(node, solidThingDef)
    {
        GenerateThingId(id);
        TicksToConstruct = ticksToConstruct;
        CurrentConstructionTicks = 0;
        RequiredMaterials = requiredMaterials;
        Inventory = new Inventory(ItemDefCount.CalculateSlotsForInventory(requiredMaterials), this, true);
        Inventory.ItemAdded += OnInventoryItemAdded;
    }

    public void OnInventoryItemAdded()
    {
        if (GetNextRequiredMaterialAmount() == null)
        {
            JobManager.EnqueueJob(new ConstructJob(this));
        }
        else
        {
            JobManager.EnqueueJob(new HaulItemToBlueprintJob(this));
        }
    }

    public ItemDefCount GetNextRequiredMaterialAmount(ItemDef materialType = null)
    {
        foreach (var requiredMaterial in RequiredMaterials)
        {
            if (materialType != null && !requiredMaterial.ItemDef.IsSameAs(materialType))
            {
                continue;
            }

            int materialContained = Inventory.GetContainedAmountOfItemType(requiredMaterial.ItemDef);

            if (materialContained >= requiredMaterial.Count)
            {
                continue;
            }

            int remainingAmountNeeded = requiredMaterial.Count - materialContained;
            return new ItemDefCount(requiredMaterial.ItemDef, remainingAmountNeeded);
        }

        return null;
    }

    public bool RequiresMaterialType(ItemDef itemDef)
    {
        foreach (var requiredMaterial in RequiredMaterials)
        {
            if (!requiredMaterial.ItemDef.IsSameAs(itemDef))
            {
                continue;
            }

            var nextMaterialCount = GetNextRequiredMaterialAmount(itemDef);
            if (nextMaterialCount != null && nextMaterialCount.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public Blueprint Place()
    {
        NodeManager.GetNodeDataAt(GetNode()).AddThing(this);
        RegionManager.AddToRegionThings(this, GetNode());
        Map.Instance.SetTile(GetNode(), ((TileDef)ThingDef).GetTile(), Map.TilemapType.BlueprintTilemap, false);

        JobManager.EnqueueJob(new HaulItemToBlueprintJob(this));

        return this;
    }

    public void FinishConstruction()
    {
        Inventory.ItemAdded -= OnInventoryItemAdded;

        NodeManager.GetNodeDataAt(GetNode()).RemoveThing(this);
        RegionManager.RemoveFromRegionThings(this, GetNode());
        Map.Instance.SetTile(GetNode(), null, Map.TilemapType.BlueprintTilemap, false);

        // Create the solid building
        ((TileDef)ThingDef).Place(GetNode(), null);
    }
}