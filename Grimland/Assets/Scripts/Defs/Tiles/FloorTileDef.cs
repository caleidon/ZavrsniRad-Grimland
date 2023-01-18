using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorTileDef : TileDef, IBuildable
{
    public int MaxHealth { get; set; }

    public List<string> ApplicableJobs { get; set; }
    public List<ItemDefCount> RequiredMaterials { get; set; }
    public int TicksToConstruct { get; set; }

    // public List<Dictionary<string, string>> RequiredMaterials { get; set; }
    // public BlueprintTileDef BlueprintTileDef { get; set; }
    // public Dictionary<string, Component> Components { get; set; } = new Dictionary<string, Component>();

    public UnityEngine.Tilemaps.Tile Tile { get; set; }

    [LoadedByYaml(YamlTag = "!FloorTile")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public int MaxHealth { get; set; }

        public string Sprite { get; set; }
        // public List<Component.Initializer> Components { get; set; }

        public List<string> ApplicableJobs { get; set; }
        public List<Dictionary<string, int>> RequiredMaterials { get; set; }
        public int TicksToConstruct { get; set; }

        public object CreateInstance()
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            tile.sprite = DefManager.Sprites[Sprite];

            FloorTileDef floorTileDef = new FloorTileDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                MaxHealth = MaxHealth,
                Tile = tile,
                RequiredMaterials = YamlUtils.ThingDefCountFromYaml(RequiredMaterials),
                TicksToConstruct = TicksToConstruct
            };

            // if (Components != null)
            // {
            //     foreach (var componentInitializer in Components)
            //     {
            //         Component component = componentInitializer.CreateInstance(floorTileDef);
            //         floorTileDef.Components.Add(component.GetType().Name, component);
            //     }
            // }
            //
            // floorTileDef.BlueprintTileDef = BlueprintTileDef.Initialize(floorTileDef, Sprite);

            return floorTileDef;
        }
    }

    // public Blueprint GetBlueprint()
    // {
    //     return (Blueprint)Components["Blueprint"];
    // }


    public override TileBase GetTile()
    {
        return Tile;
    }

    public override Tile Place(Vector3Int node, Tile existingTile, bool loadingMode = false)
    {
        FloorTile floorTile = (FloorTile)existingTile ?? new FloorTile(node, this);
        NodeManager.GetNodeDataAt(node).AddThing(floorTile);
        Map.Instance.SetTile(node, Tile, Map.TilemapType.FloorTilemap, loadingMode, new Color(0.375f, 0.25f, 0.179f, 1f));

        return floorTile;
    }

    public static void LoadInflatedData(string[] data)
    {
        foreach (var node in Map.AllNodes())
        {
            if (data[node.y * Map.Instance.Size.x + node.x] == "-")
            {
                continue;
            }

            string[] floorTileDataSplit = data[node.y * Map.Instance.Size.x + node.x].Split(',');

            string defName = floorTileDataSplit[0];
            int health = int.Parse(floorTileDataSplit[1]);

            FloorTileDef floorTileDef = DefManager.Defs.Value<FloorTileDef>(defName);
            FloorTile floorTile = (FloorTile)floorTileDef.Place(node, null, true);
            floorTile.Health = health;
        }
    }

    public Blueprint PlaceBlueprint(Vector3Int node)
    {
        Blueprint blueprint = new Blueprint(null, node, this, RequiredMaterials, TicksToConstruct);
        return blueprint.Place();
    }
}

public class FloorTile : Tile, ICanBeDeflationSaved
{
    public override bool IsRegionThing => false;
    public int Health { get; set; }

    public FloorTile(Vector3Int node, FloorTileDef floorTileDef, int? health = null) : base(node, floorTileDef)
    {
        Health = health ?? floorTileDef.MaxHealth;
    }

    public string GetDeflationString()
    {
        return $"{ThingDef.Name},{Health}/";
    }
}