using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FindOptionalItemsAndBlueprints : GrimAction
{
    public SharedInt ExportGloballyWantedAmount;

    private Item initialItem;
    private Blueprint initialBlueprint;

    public override void OnStart()
    {
        base.OnStart();

        initialItem = ((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp[0];
        initialBlueprint = ((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo[0];
    }

    public override TaskStatus OnUpdate()
    {
        List<Item> potentialAdditionalItems = new List<Item>();
        List<Blueprint> potentialAdditionalBlueprints = new List<Blueprint>();

        potentialAdditionalItems.Add(((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp[0]);
        ((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.Clear();

        potentialAdditionalBlueprints.Add(((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo[0]);
        ((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.Clear();

        // TODO: this should maybe use regionthings for higher performance
        var possibleBlueprintNodes = FloodFill.FloodAllTiles(initialBlueprint.GetNode(), 6);

        foreach (var node in possibleBlueprintNodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            if (!nodeData.TryGetBlueprint(out Blueprint blueprint) || blueprint == initialBlueprint)
            {
                continue;
            }

            if (!blueprint.RequiresMaterialType(initialItem.GetItemDef()))
            {
                continue;
            }

            if (!ReservationManager.CanReserve(blueprint, ReservationType.HaulTo, out Creature owner))
            {
                continue;
            }

            potentialAdditionalBlueprints.Add(blueprint);
        }

        var possibleItemNodes = FloodFill.FloodAllTiles(initialItem.GetNode(), 3);

        foreach (var node in possibleItemNodes)
        {
            NodeData nodeData = NodeManager.GetNodeDataAt(node);

            if (!nodeData.TryGetItem(out Item existingItem))
            {
                continue;
            }

            if (!existingItem.HasSameDefAs(initialItem) || existingItem == initialItem)
            {
                continue;
            }

            if (!ReservationManager.CanReserve(existingItem, ReservationType.HaulFrom, out Creature owner))
            {
                continue;
            }

            potentialAdditionalItems.Add(existingItem);
        }

        List<KeyValuePair<Item, int>> itemAmounts = new List<KeyValuePair<Item, int>>();

        for (int i = 0; i < potentialAdditionalItems.Count; i++)
        {
            Item potentialItem = potentialAdditionalItems[i];
            itemAmounts.Add(new KeyValuePair<Item, int>(potentialItem, potentialItem.Amount));
        }

        int globallyWantedAmount = 0;

        foreach (var potentialBlueprint in potentialAdditionalBlueprints)
        {
            globallyWantedAmount += potentialBlueprint.GetNextRequiredMaterialAmount(initialItem.GetItemDef()).Count;
        }

        while (potentialAdditionalBlueprints.Count > 0 && itemAmounts.Count > 0)
        {
            Blueprint blueprintToExamine = potentialAdditionalBlueprints[0];
            potentialAdditionalBlueprints.RemoveAt(0);

            int blueprintMaterialNeeded = blueprintToExamine.GetNextRequiredMaterialAmount(initialItem.GetItemDef()).Count;
            int blueprintMaterialSecured = 0;

            while (blueprintMaterialSecured < blueprintMaterialNeeded && itemAmounts.Count > 0)
            {
                Item potentialItem = itemAmounts[0].Key;
                int amountLeft = itemAmounts[0].Value;
                itemAmounts.RemoveAt(0);

                int blueprintMaterialWanted = blueprintMaterialNeeded - blueprintMaterialSecured;

                if (amountLeft > blueprintMaterialWanted)
                {
                    int difference = blueprintMaterialWanted;
                    blueprintMaterialSecured += difference;
                    itemAmounts.Insert(0, new KeyValuePair<Item, int>(potentialItem, amountLeft - difference));
                }

                if (amountLeft <= blueprintMaterialWanted)
                {
                    blueprintMaterialSecured += amountLeft;
                }

                ReservationManager.TryReserve(Creature, blueprintToExamine, CurrentJob, ReservationType.HaulTo);
                ReservationManager.TryReserve(Creature, potentialItem, CurrentJob, ReservationType.HaulFrom);

                if (!((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.Contains(potentialItem))
                {
                    ((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.Add(potentialItem);
                }

                if (!((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.Contains(blueprintToExamine))
                {
                    ((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.Add(blueprintToExamine);
                }

                var haulToBlueprintJobs = JobManager.FindJobOfType<HaulItemToBlueprintJob>();

                var blueprintJobToRemove = haulToBlueprintJobs.FirstOrDefault(job => job.Blueprint.Id == blueprintToExamine.Id);

                if (blueprintJobToRemove != null)
                {
                    JobManager.DequeueJob(blueprintJobToRemove);
                }
            }
        }


        ExportGloballyWantedAmount.Value = globallyWantedAmount;

        Debug.Log(
            $"Globally, we want to take {globallyWantedAmount} items ({((HaulItemToBlueprintJob) CurrentJob).ItemsToPickUp.Count}) to {((HaulItemToBlueprintJob) CurrentJob).BlueprintsToHaulTo.Count} blueprints");

        return TaskStatus.Success;
    }
}