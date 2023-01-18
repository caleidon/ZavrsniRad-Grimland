using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConnectableTileDef : TileDef
{
    public Color Color { get; set; }
    public ConnectableRuleTile RuleTile { get; set; }

    [LoadedByYaml(YamlTag = "!ConnectableTile")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public string Color { get; set; }
        public string Atlas { get; set; }

        public object CreateInstance()
        {
            ConnectableRuleTile connectableRuleTile = DefManager.RuleTiles[Atlas];

            ConnectableTileDef connectableTileDef = new ConnectableTileDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                Color = YamlUtils.ColorFromYaml(Color),
                RuleTile = connectableRuleTile
            };

            return connectableTileDef;
        }
    }

    public override TileBase GetTile()
    {
        return RuleTile;
    }

    public override Tile Place(Vector3Int node, Tile existingTile, bool loadingMode = false)
    {
        ConnectableTile connectableTile = (ConnectableTile)existingTile ?? new ConnectableTile(null, node, this);
        NodeManager.GetNodeDataAt(node).AddThing(connectableTile);

        if (!loadingMode)
        {
            RegionManager.AddToRegionThings(connectableTile, node);
        }

        Map.Instance.SetTile(node, RuleTile, Map.TilemapType.BaseTilemap, loadingMode, Color);
        return connectableTile;
    }
}

public class ConnectableTile : Tile
{
    public override bool IsRegionThing => true;

    public ConnectableTile([CanBeNull] string id, Vector3Int node, ThingDef connectableTileDef) : base(node, connectableTileDef)
    {
        GenerateThingId(id);
    }
}
//
// public class BuildingSaver : ISaver
// {
//     public string ID { get; set; }
//     public string BuildingTileName { get; set; }
//     public Vector3Int Location { get; set; }
//     public int HP { get; set; }
//
//     public BuildingSaver(BuildingTile buildingTile)
//     {
//         ID = buildingTile.Id;
//         BuildingTileName = buildingTile.BuildingTileDef.Name;
//         Location = buildingTile.Location;
//         HP = buildingTile.HP;
//     }
//
//     public BuildingSaver() { }
//
//     public BuildingTile RecreateThing()
//     {
//         BuildingTile buildingTile = (BuildingTile) DefManager.Defs.Value<BuildingTileDef>(BuildingTileName).Place(Location);
//         buildingTile.Id = ID;
//         buildingTile.HP = HP;
//         return buildingTile;
//     }
//
//
//     public void Load()
//     {
//         RecreateThing();
//     }
//
//     public T RecreateThing<T>() where T : class
//     {
//         throw new NotImplementedException();
//     }
// }