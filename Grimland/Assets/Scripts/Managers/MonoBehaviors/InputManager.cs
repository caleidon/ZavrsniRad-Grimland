using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private int itemCount;

    public static void SpawnWood(InputAction.CallbackContext context)
    {
        Vector3Int node = Map.GridPosFromMousePos();

        NodeData nodeData = NodeManager.GetNodeDataAt(node);

        foreach (var thing in nodeData.Things)
        {
            if (thing is VegetationTile vegetationTile)
            {
                JobManager.EnqueueJob(new CutVegetationJob(vegetationTile));
            }
        }
    }

    public static void SpawnBlueprint(InputAction.CallbackContext context)
    {
        Vector3Int node = Map.GridPosFromMousePos();

        DefManager.Defs.Value<IBuildable>("Grass").PlaceBlueprint(node);
    }

    public static void SpawnHuman(InputAction.CallbackContext context)
    {
        Vector3Int node = Map.GridPosFromMousePos();

        DefManager.Defs.Value<HumanDef>("Human").SpawnCreature(node);
    }

    public static void SpawnWater(InputAction.CallbackContext context)
    {
        // if (SelectionManager.Instance.currentMode != SelectionManager.Mode.None)
        // {
        //     return;
        // }
        //
        // Vector3Int node = Map.GridPosFromMousePos();
        // var nodeData = NodeManager.GetNodeDataAt(node);
        // foreach (var thing in nodeData.Things)
        // {
        //     if (thing is BaseTile baseTile)
        //     {
        //         if (!((TileDef)baseTile.ThingDef).IsWalkable)
        //         {
        //             return;
        //         }
        //     }
        // }
        //
        // DefManager.Defs.Value<TileDef>("DeepWater").Place(node, null);
    }

    public static void SpawnSoil(InputAction.CallbackContext context)
    {
        // Vector3Int node = Map.GridPosFromMousePos();
        // var nodeData = NodeManager.GetNodeDataAt(node);
        // foreach (var thing in nodeData.Things)
        // {
        //     if (thing is BaseTile baseTile)
        //     {
        //         if (((TileDef)baseTile.ThingDef).IsWalkable)
        //         {
        //             return;
        //         }
        //     }
        // }
        //
        // DefManager.Defs.Value<TileDef>("Soil").Place(node, null);
    }

    private void Update()
    {
        if (UIManager.IsMouseOverUI())
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SelectionManager.Instance.OnLmbClick();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectionManager.Instance.OnLmbRelease();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            SelectionManager.Instance.OnLmbHold();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SelectionManager.Instance.OnRmbClick();
        }


        // if (Keyboard.current[Key.H].wasPressedThisFrame)
        // {
        //     Material wood = (Material)DefManager.Defs.Value<MaterialDef>("Wood").Create(16);
        //     DefManager.Defs.Value<MaterialDef>("Wood").PlaceInWorld(Map.GridPosFromMousePos(), wood, true);
        //     itemCount += wood.Amount;
        // }

        //
        // if (Keyboard.current[Key.T].wasPressedThisFrame)
        // {
        //     Material wood = (Material) DefManager.Defs.Value<MaterialDef>("Wood").Create(1);
        //     DefManager.Defs.Value<MaterialDef>("Wood").PlaceInWorld(Map.GridPosFromMousePos(), wood, true);
        //     itemCount += wood.Amount;
        // }
        //
        if (Keyboard.current[Key.Q].isPressed)
        {
            Vector3Int node = Map.GridPosFromMousePos();
            var nodeData = NodeManager.GetNodeDataAt(node);
            foreach (var thing in nodeData.Things)
            {
                if (thing is BaseTile baseTile)
                {
                    if (!((TileDef)baseTile.ThingDef).IsWalkable)
                    {
                        return;
                    }
                }
            }

            DefManager.Defs.Value<TileDef>("DeepWater").Place(node, null);
        }

        if (Keyboard.current[Key.E].isPressed)
        {
            Vector3Int node = Map.GridPosFromMousePos();
            var nodeData = NodeManager.GetNodeDataAt(node);
            foreach (var thing in nodeData.Things)
            {
                if (thing is BaseTile baseTile)
                {
                    if (((TileDef)baseTile.ThingDef).IsWalkable)
                    {
                        return;
                    }
                }
            }

            DefManager.Defs.Value<TileDef>("Soil").Place(node, null);
        }
        //
        // if (Keyboard.current[Key.J].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //
        //     DefManager.Defs.Value<IBuildable>("Grass").PlaceBlueprint(node);
        // }
        //
        // if (Keyboard.current[Key.K].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //     Blueprint blueprint = (Blueprint) NodeManager.GetNodeDataAt(node).Things.FirstOrDefault(thing => thing is Blueprint);
        //
        //     blueprint?.FinishConstruction();
        // }

        // if (Keyboard.current[Key.J].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //     var nodeData = NodeManager.GetNodeDataAt(node);
        //     if (nodeData.TryGetItem(out Item item)) { }
        //
        //     Creature creature = (Creature)IdManager.GetAllThings().First(thing => thing is Creature);
        //     creature.ItemInHandTracker.TryTakeItem(item, item.Amount);
        // }
        //
        // if (Keyboard.current[Key.K].wasPressedThisFrame)
        // {
        //     Creature creature = (Creature)IdManager.GetAllThings().First(thing => thing is Creature);
        //     creature.ItemInHandTracker.DropItemOnFloor();
        // }

        // if (Keyboard.current[Key.J].wasPressedThisFrame)
        // {
        //     Vector3Int center = Map.GridPosFromMousePos();
        //
        //     int radius = Random.Range(3, 11);
        //
        //     Debug.Log($"Radii with radius of {radius}");
        //
        //     GizmoManager.radiiNodes.Clear();
        //
        //     foreach (var node in RadialPattern.NodesInRadius(center, radius, true))
        //     {
        //         GizmoManager.radiiNodes.Add(node);
        //     }
        // }


        // if (Keyboard.current[Key.Y].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //
        //     NodeData nodeData = NodeManager.GetNodeDataAt(node);
        //
        //     foreach (var thing in nodeData.Things)
        //     {
        //         if (thing is VegetationTile vegetationTile)
        //         {
        //             JobManager.EnqueueJob(new CutVegetationJob(vegetationTile));
        //         }
        //     }
        // }
        //
        // if (Keyboard.current[Key.U].wasPressedThisFrame)
        // {
        //     int totalItemCount = 0;
        //
        //     foreach (var thing in IdManager.GetAllThings())
        //     {
        //         if (thing is Item item)
        //         {
        //             totalItemCount += item.Amount;
        //         }
        //     }
        //
        //     string success = totalItemCount == itemCount ? "SUCCESS" : "FAILURE";
        //     Debug.Log($"{success} (World: {totalItemCount}) (Spawned: {itemCount})");
        // }
        //
        // if (Keyboard.current[Key.I].wasPressedThisFrame)
        // {
        //     foreach (var job in JobManager.availableJobs)
        //     {
        //         Debug.Log($"There is an available job - {job}");
        //     }
        //
        //     // int count = 0;
        //     //
        //     // foreach (var thing in IdManager.GetAllThings())
        //     // {
        //     //     if (thing is Blueprint blueprint)
        //     //     {
        //     //         if (!blueprint.isBuilt)
        //     //         {
        //     //             count++;
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // if (count > 0)
        //     // {
        //     //     Debug.LogError($"WARNING THERE ARE {count} UNBUILT BLUEPRINTS");
        //     // }
        // }
        //
        // if (Keyboard.current[Key.O].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //
        //     DefManager.Defs.Value<HumanDef>("Human").SpawnCreature(node);
        // }
        //
        //
        // if (Keyboard.current[Key.P].wasPressedThisFrame)
        // {
        //     Vector3Int node = Map.GridPosFromMousePos();
        //     NodeData nodeData = NodeManager.GetNodeDataAt(node);
        //
        //     if (nodeData.TryGetItem(out Item item))
        //     {
        //         Debug.Log($"Item: {item} with amount {item.Amount}");
        //     }
        //
        //     if (nodeData.TryGetBlueprint(out Blueprint blueprint))
        //     {
        //         Debug.Log("IN BLUEPRINT:");
        //         foreach (var inventoryItem in blueprint.Inventory.Items)
        //         {
        //             Debug.Log($"Item: {inventoryItem} with amount {inventoryItem.Amount}");
        //         }
        //     }
        // }
    }
}