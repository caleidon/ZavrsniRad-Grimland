using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VegetationTileDef : TileDef
{
    public int MaxHealth { get; set; }
    public List<Dictionary<string, string>> DropOnDestroy { get; set; }
    public UnityEngine.Tilemaps.Tile Tile { get; set; }

    [LoadedByYaml(YamlTag = "!VegetationTile")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public int MaxHealth { get; set; }
        public List<string> GrowthSprites = new List<string>();
        public List<Dictionary<string, string>> DropOnDestroy { get; set; } = new List<Dictionary<string, string>>();
        public List<string> ApplicableJobs { get; set; }

        public object CreateInstance()
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            tile.sprite = DefManager.Sprites[GrowthSprites[0]];

            VegetationTileDef vegetationTileDef = new VegetationTileDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                MaxHealth = MaxHealth,
                Tile = tile
            };

            if (DropOnDestroy.Count > 0)
            {
                vegetationTileDef.DropOnDestroy = DropOnDestroy;
            }

            return vegetationTileDef;
        }
    }

    public override Tile Place(Vector3Int node, Tile existingTile, bool loadingMode = false)
    {
        VegetationTile vegetationTile = (VegetationTile) existingTile ?? new VegetationTile(null, node, this);

        NodeManager.GetNodeDataAt(node).AddThing(vegetationTile);

        if (!loadingMode)
        {
            RegionManager.AddToRegionThings(vegetationTile, node);
        }

        Map.Instance.SetTile(node, Tile, Map.TilemapType.VegetationTilemap, loadingMode);
        return vegetationTile;
    }

    public override TileBase GetTile()
    {
        return Tile;
    }
}

public class VegetationTile : Tile, ICanBeSaved
{
    public float GrowthProgress { get; set; }
    public override bool IsRegionThing => true;
    public int Health { get; set; }

    public VegetationTile([CanBeNull] string id, Vector3Int node, VegetationTileDef vegetationTileDef) : base(node, vegetationTileDef)
    {
        GenerateThingId(id);
        SubscribeToTicks();
        GrowthProgress = 0f;
        Health = vegetationTileDef.MaxHealth;
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);

        Health -= damage;

        if (Health <= 0)
        {
            Destroy(DestroyMode.Normal);
        }
    }

    public override void Destroy(DestroyMode destroyMode)
    {
        base.Destroy(destroyMode);

        NodeManager.GetNodeDataAt(Node).RemoveThing(this);
        RegionManager.RemoveFromRegionThings(this, Node);

        Map.Instance.SetTile(Node, null, Map.TilemapType.VegetationTilemap, true);

        foreach (var drop in ((VegetationTileDef) ThingDef).DropOnDestroy)
        {
            // TODO: if there is already an item here you should drop next to it
            // TODO: what if you drop more than max stack size, also drop to next free spot
            // TODO: make this into a function or a class to handle, cuz its always the same
            int numberOfDrops = YamlUtils.GetRandomIntFromRange(drop["MinAmount"], drop["MaxAmount"]);

            if (numberOfDrops > 0)
            {
                Item itemToDrop = DefManager.Defs.Value<ItemDef>(drop["ItemName"]).Create(numberOfDrops);
                Vector3Int dropNode = CreatureItemTracker.FindFreeDropLocation(GetNode());
                DefManager.Defs.Value<ItemDef>(drop["ItemName"]).PlaceInWorld(dropNode, itemToDrop, true);
            }
        }
    }

    public ISaver GetSaver()
    {
        return new VegetationTileSaver(this);
    }
}

public class VegetationTileSaver : ISaver
{
    public string Id { get; set; }
    public Vector3Int Node { get; set; }
    public string VegetationDefName { get; set; }
    public int Health { get; set; }
    public float GrowthProgress { get; set; }

    public VegetationTileSaver(VegetationTile vegetationTile)
    {
        Node = vegetationTile.GetNode();
        VegetationDefName = vegetationTile.ThingDef.Name;
        GrowthProgress = vegetationTile.GrowthProgress;
        Health = vegetationTile.Health;
    }

    public VegetationTileSaver() { }

    private VegetationTileDef vegetationTileDef;
    private VegetationTile vegetationTile;

    public void Load(ISaver.Phase phase)
    {
        switch (phase)
        {
            case ISaver.Phase.Create:
                vegetationTileDef = DefManager.Defs.Value<VegetationTileDef>(VegetationDefName);
                vegetationTile = new VegetationTile(Id, Node, vegetationTileDef)
                {
                    GrowthProgress = GrowthProgress,
                    Health = Health
                };
                break;
            case ISaver.Phase.Instantiate:
                vegetationTileDef.Place(Node, vegetationTile, true);
                break;
        }
    }
}