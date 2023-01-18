using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseTileDef : TileDef
{
    public float Fertility { get; set; }
    public float GenHeight { get; set; }

    public UnityEngine.Tilemaps.Tile Tile { get; set; }

    [LoadedByYaml(YamlTag = "!BaseTile")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public float Fertility { get; set; }
        public float GenHeight { get; set; }
        public string Sprite { get; set; }


        public object CreateInstance()
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            tile.sprite = DefManager.Sprites[Sprite];

            BaseTileDef baseTileDef = new BaseTileDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                Fertility = Fertility,
                GenHeight = GenHeight,
                Tile = tile
            };

            Debug.Log($"Created tile {Name} with height {GenHeight}");

            return baseTileDef;
        }
    }

    public override Tile Place(Vector3Int node, Tile existingTile, bool loadingMode = false)
    {
        BaseTile baseTile = (BaseTile)existingTile ?? new BaseTile(node, this);
        // TODO Remove clearthings
        NodeManager.GetNodeDataAt(node).Things.Clear();
        NodeManager.GetNodeDataAt(node).AddThing(baseTile);
        Map.Instance.SetTile(node, Tile, Map.TilemapType.BaseTilemap, loadingMode);
        return baseTile;
    }

    public static void LoadInflatedData(string[] baseTileData)
    {
        foreach (var node in Map.AllNodes())
        {
            string baseTileName = baseTileData[node.y * Map.Instance.Size.x + node.x];
            BaseTileDef baseTileDef = DefManager.Defs.Value<BaseTileDef>(baseTileName);
            baseTileDef.Place(node, null, true);
        }
    }

    public override TileBase GetTile()
    {
        return Tile;
    }
}

public class BaseTile : Tile, ICanBeDeflationSaved
{
    public override bool IsDamagable => false;
    public override bool IsRegionThing => false;
    public BaseTile(Vector3Int node, ThingDef tileDef) : base(node, tileDef) { }

    public string GetDeflationString()
    {
        return $"{ThingDef.Name}/";
    }
}