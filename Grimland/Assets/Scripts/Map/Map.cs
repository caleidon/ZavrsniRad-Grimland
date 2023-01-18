using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommandTerminal;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Map
{
    public static Map Instance { get; private set; }

    public enum TilemapType
    {
        BaseTilemap,
        FloorTilemap,
        VegetationTilemap,
        BlueprintTilemap,
        BuildingTilemap,
        ItemTilemap
    }

    // Map data
    public Vector2Int Size { get; set; }
    private Rect Rect { get; set; }

    // Tilemap (world) data
    private GameObject Grid { get; set; }
    private Tilemap BaseTilemap { get; set; }
    private Tilemap FloorTilemap { get; set; }
    private Tilemap VegetationTilemap { get; set; }
    private Tilemap BlueprintTilemap { get; set; }
    private Tilemap BuildingTilemap { get; set; }
    private Tilemap ItemTilemap { get; set; }

    public static event Action OnTileUpdate;


    public Map(int width, int height)
    {
        Instance = this;
        Size = new Vector2Int(width, height);

        InitializeMap();
    }

    public Map()
    {
        Instance = this;
    }

    private void InitializeMap()
    {
        Rect = new Rect(Vector2.zero, Size);

        NodeManager.Reset(Size);
        IdManager.Reset();
        ZoneManager.Reset();
        RegionManager.Reset();
        RoomManager.Reset();
        JobManager.Reset();
        ReservationManager.Reset();
        HaulReservationManager.Reset();
        TickManager.ResetTickManager();

        OnTileUpdate = null;

        CreateWorldGrid();
    }


    public IEnumerator Generate()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        TickManager.PauseGame();
        LoadingManager.SetLoading(true, LoadingManager.LoadingType.Generation);

        yield return null;

        float[,] noiseMap = NoiseGenerator.GenerateTerrainNoiseMap(Size.x, Size.y);
        float[,] soilNoiseMap = NoiseGenerator.GenerateSoilNoiseMap(Size.x, Size.y);

        // Map generation
        GenerateBaseTiles(noiseMap, soilNoiseMap);
        GenerateVegetation();

        // Regions and rooms
        RegionManager.GenerateRegions();
        RegionManager.GenerateRegionThings();
        RoomManager.GenerateRooms();


        LoadingManager.SetLoading(false);

        // Start the time, enable controls and UI, and then activate Gstar
        TickManager.ResumeGame();
        ControlsManager.EnableGameInput();
        ControlsManager.Controls.GameControls.Enable();
        UIManager.Instance.SetGameUI(true);

        PathfindingManager.InitializeGstar();

        stopwatch.Stop();

        Debug.Log($"Elapsed Time is {stopwatch.ElapsedMilliseconds} ms");
    }

    private void GenerateBaseTiles(float[,] noiseMap, float[,] soilNoiseMap)
    {
        foreach (var node in AllNodes())
        {
            BaseTileDef baseTileDef = DefManager.GetBaseTileByHeight(noiseMap[node.x, node.y]);

            // if (baseTileDef.Name == "Soil")
            // {
            //     DefManager.Defs.Value<BaseTileDef>("Soil").Place(node, null, true);
            //     // GenerateSoilTiles(node, soilNoiseMap);
            // }
            // else
            // {
            Terminal.Log($"Spawning {baseTileDef.Name} at height {noiseMap[node.x, node.y]}");
            baseTileDef.Place(node, null, true);
            // }
        }
    }


    private void GenerateVegetation()
    {
        foreach (var nodeData in NodeManager.AllNodeData())
        {
            if (nodeData.GetFertility() > 0)
            {
                if (Random.Range(0f, 1f) > 0.9f)
                {
                    DefManager.Defs.Value<VegetationTileDef>("Tree").Place(nodeData.Node, null, true);
                }
            }
        }
    }


    private void GenerateSoilTiles(Vector3Int node, float[,] soilNoiseMap)
    {
        if (soilNoiseMap[node.x, node.y] <= 0.2f)
        {
            DefManager.Defs.Value<BaseTileDef>("DrySoil").Place(node, null, true);
        }
        else if (soilNoiseMap[node.x, node.y] <= 0.8f)
        {
            DefManager.Defs.Value<BaseTileDef>("Soil").Place(node, null, true);
            DefManager.Defs.Value<FloorTileDef>("Grass").Place(node, null, true);
        }
        else
        {
            DefManager.Defs.Value<BaseTileDef>("FertileSoil").Place(node, null, true);
            DefManager.Defs.Value<FloorTileDef>("Grass").Place(node, null, true);
        }
    }

    public IEnumerator LoadMap()
    {
        TickManager.PauseGame();
        LoadingManager.SetLoading(true, LoadingManager.LoadingType.Loading);

        yield return null;

        // Make sure to initialize map before loading anything, since it clears cached information
        InitializeMap();

        TickManager.Tick = SaveManager.TickManagerSaver.Tick;
        SaveManager.IdManagerSaver.Load();

        BaseTileDef.LoadInflatedData(SavingUtils.Inflate(SaveManager.NodeManagerSaver.DeflatedBase));
        FloorTileDef.LoadInflatedData(SavingUtils.Inflate(SaveManager.NodeManagerSaver.DeflatedFloor));

        foreach (var saveData in SaveManager.NodeManagerSaver.ThingSavers)
        {
            saveData.Load(ISaver.Phase.Create);
        }

        foreach (var saveData in SaveManager.NodeManagerSaver.ThingSavers)
        {
            saveData.Load(ISaver.Phase.Link);
        }

        foreach (var saveData in SaveManager.NodeManagerSaver.ThingSavers)
        {
            saveData.Load(ISaver.Phase.Instantiate);
        }

        RegionManager.GenerateRegions();
        RegionManager.GenerateRegionThings();
        RoomManager.GenerateRooms();

        // TODO: Calculate regions after load
        // TODO: make sure after exiting map to main menu that ALL data is destroyed, because regions seemed to remain? same for existing Ids in ThingList

        LoadingManager.SetLoading(false);

        ControlsManager.EnableGameInput();
        UIManager.Instance.SetGameUI(true);
        ControlsManager.Controls.GameControls.Enable();
        TickManager.ResumeGame();

        PathfindingManager.InitializeGstar();
    }

    public static IEnumerable<Vector3Int> AllNodes()
    {
        Map map = Instance;

        for (int y = (int)map.Rect.y; y < map.Rect.yMax; y++)
        {
            for (int x = (int)map.Rect.x; x < map.Rect.xMax; x++)
            {
                yield return new Vector3Int(x, y, 0);
            }
        }
    }


    private void CreateWorldGrid()
    {
        Grid = new GameObject("World Grid");
        Grid.AddComponent<Grid>().cellSize = new Vector3(1, 1, 0);

        CreateTilemaps();
    }

    private void CreateTilemaps()
    {
        foreach (var layer in Enum.GetNames(typeof(TilemapType)))
        {
            GameObject tilemapGO = new GameObject(layer);
            Tilemap tilemap = tilemapGO.AddComponent<Tilemap>();

            TilemapRenderer renderer = tilemapGO.AddComponent<TilemapRenderer>();

            tilemapGO.transform.SetParent(Grid.gameObject.transform);
            tilemap.tileAnchor = new Vector3(0, 0, 0);

            int tilemapID = (int)Enum.Parse(typeof(TilemapType), layer);
            renderer.sortingOrder = (int)Enum.Parse(typeof(Settings.LayerOrder), layer);
            renderer.sortOrder = TilemapRenderer.SortOrder.TopLeft;

            switch (tilemapID)
            {
                case 0:
                    BaseTilemap = tilemap;
                    break;
                case 1:
                    FloorTilemap = tilemap;
                    break;
                case 2:
                    VegetationTilemap = tilemap;
                    break;
                case 3:
                    BlueprintTilemap = tilemap;
                    var bpTilemapRenderer = BlueprintTilemap.GetComponent<TilemapRenderer>();
                    bpTilemapRenderer.material = PrefabManager.Instance.Material;
                    break;
                case 4:
                    BuildingTilemap = tilemap;
                    break;
                case 5:
                    ItemTilemap = tilemap;
                    break;
            }
        }
    }

    /*public void TryPlaceDoor(Vector3Int node)
    {
        switch (nodeData[node.x, node.y].type)
        {
            case NodeData.Type.Door:
                Debug.Log("There is already a door there.");
                return;
            case NodeData.Type.Obstacle:
                Debug.Log("You cannot place doors on obstacles.");
                return;
            case NodeData.Type.Walkable:
                Region.RemoveNode(node);
                DoorRegion.Place(node, CreateRegionRectangle(node.x, node.y));
                GameObject doorEntity = DefManager.doors["DoorWood"].Create(node);
                GameObjectsArray[node.x, node.y] = doorEntity;
                nodeData[node.x, node.y].Entity = doorEntity.GetComponent<Door>();
                break;
        }
    }#1#*/


    public void SetTile(Vector3Int node, TileBase tile, TilemapType tilemapType, bool loadingMode, Color color = default)
    {
        if (color == default)
        {
            color = Color.white;
        }

        switch (tilemapType)
        {
            case TilemapType.BaseTilemap:
                BaseTilemap.SetTile(node, tile);
                BaseTilemap.SetColor(node, color);
                break;
            case TilemapType.FloorTilemap:
                FloorTilemap.SetTile(node, tile);
                FloorTilemap.SetTileFlags(node, TileFlags.None);
                FloorTilemap.SetColor(node, color);
                break;
            case TilemapType.VegetationTilemap:
                VegetationTilemap.SetTile(node, tile);
                VegetationTilemap.SetColor(node, color);
                break;
            case TilemapType.BlueprintTilemap:
                BlueprintTilemap.SetTile(node, tile);
                break;
            case TilemapType.BuildingTilemap:
                BuildingTilemap.SetTile(node, tile);
                break;
            case TilemapType.ItemTilemap:
                ItemTilemap.SetTile(node, tile);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tilemapType), tilemapType, null);
        }

        if (!loadingMode)
        {
            RegionManager.UpdateRegion(node);
            PathfindingManager.UpdateGraphPosition(node);
        }


        // TODO: ontileupdate should only invoke if walkability was changed, we only need to change it always if traversalspeedcalculator depends on it
        OnTileUpdate?.Invoke();
    }


    public static bool Contains(Vector3Int node)
    {
        return Instance.Rect.Contains(node);
    }


    public bool GetClosestWalkableNode(Vector3Int startingNode, out Vector3Int walkableNode, int maxSearchRange = 2)
    {
        if (!Contains(startingNode))
        {
            walkableNode = Vector3Int.zero;
            return false;
        }

        if (NodeManager.GetNodeDataAt(startingNode).IsWalkable)
        {
            walkableNode = startingNode;
            return true;
        }

        AstarPath.active.maxNearestNodeDistance = maxSearchRange + 1;
        var nearestNode = AstarPath.active.GetNearest(GridPosFromMousePos(), NNConstraint.Default).node;
        if (nearestNode != null)
        {
            walkableNode = Vector3Int.FloorToInt((Vector3)nearestNode.position);
            return true;
        }

        walkableNode = Vector3Int.zero;
        return false;
    }

    // public Vector3Int GetRandomGlobalNode(bool walkable = true)
    // {
    //     if (!walkable)
    //     {
    //         return new Vector3Int(Random.Range(0, Size.x), Random.Range(0, Size.y), 0);
    //     }
    //
    //     HashSet<Vector3Int> walkableNodes = new HashSet<Vector3Int>();
    //     foreach (var region in Regions.Values)
    //     {
    //         walkableNodes.UnionWith(region.Nodes);
    //     }
    //
    //     return walkableNodes.ElementAt(Random.Range(0, walkableNodes.Count));
    // }


    public bool GetRandomRegionNode(Vector3Int startingNode, bool includeNeighborRegions, out Vector3Int randomNode)
    {
        // TODO: ignore slow things like trees in getting random node OR apply heavy penalty

        randomNode = startingNode;

        if (!RegionManager.GetRegionFromNode(startingNode, out Region startingRegion) || startingRegion.Nodes.Count <= 1)
        {
            return false;
        }

        if (includeNeighborRegions)
        {
            HashSet<Vector3Int> combinedNodes = new HashSet<Vector3Int>();
            combinedNodes.UnionWith(startingRegion.Nodes);
            foreach (var neighborRegion in startingRegion.GetNeighbors())
            {
                combinedNodes.UnionWith(neighborRegion.Nodes);
            }

            combinedNodes.Remove(startingNode);

            randomNode = combinedNodes.ElementAt(Random.Range(0, combinedNodes.Count));
            return true;
        }

        while (randomNode == startingNode)
        {
            randomNode = startingRegion.Nodes.ElementAt(Random.Range(0, startingRegion.Nodes.Count));
        }

        return true;
    }

    public static Vector3Int GridPosFromMousePos()
    {
        return Instance.BaseTilemap.WorldToCell(Utilities.MouseWorldPosition() + new Vector2(0.5f, 0.5f));
    }

    public static Vector3Int GridPosFromWorldPos(Vector2 worldPosition)
    {
        return Instance.BaseTilemap.WorldToCell(worldPosition + new Vector2(0.5f, 0.5f));
    }

    public static void Destroy()
    {
        Object.Destroy(Instance.Grid);
        Instance = null;
    }
}