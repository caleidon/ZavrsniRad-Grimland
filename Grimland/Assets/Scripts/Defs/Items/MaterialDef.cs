using JetBrains.Annotations;
using UnityEngine;

public class MaterialDef : ItemDef
{
    [LoadedByYaml(YamlTag = "!Material")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public int MaxStackSize { get; set; }
        public int MaxHealth { get; set; }
        public string Sprite { get; set; }

        public object CreateInstance()
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            tile.sprite = DefManager.Sprites[Sprite];

            MaterialDef materialDef = new MaterialDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                MaxStackSize = MaxStackSize,
                MaxHealth = MaxHealth,
                Tile = tile
            };

            return materialDef;
        }
    }

    public override Item Create(int amount)
    {
        if (amount > MaxStackSize)
        {
            Debug.LogError($"Tried to create item {DisplayName} with amount greater than MaxStackSize.");
        }

        if (amount <= 0)
        {
            Debug.LogError($"Tried to create item {DisplayName} with amount smaller than or equal to zero.");
        }

        Material material = new Material(null, this, amount, Item.ItemState.None);
        return material;
    }
}


public class Material : Item
{
    public Material([CanBeNull] string id, ItemDef foodDef, int amount, ItemState state) : base(id, foodDef, amount, state)
    {
        Health = foodDef.MaxHealth;
    }

    public override Item MakeCopy()
    {
        Material newMaterial = new Material(null, (MaterialDef)ThingDef, Amount, State)
        {
            Health = Health
        };

        return newMaterial;
    }

    public override ISaver GetSaver()
    {
        return new MaterialSaver(this);
    }
}

public class MaterialSaver : ISaver
{
    public string Id { get; set; }
    public string MaterialDefName { get; set; }
    public int Health { get; set; }
    public int Amount { get; set; }
    public string InventoryId { get; set; }

    public MaterialSaver(Material material)
    {
        Id = material.Id;
        MaterialDefName = material.ThingDef.Name;
        Health = material.Health;
        Amount = material.Amount;
        // InventoryId = material.Inventory.Id;
    }

    public MaterialSaver() { }

    private MaterialDef materialDef;
    private Material material;

    public void Load(ISaver.Phase phase)
    {
        switch (phase)
        {
            case ISaver.Phase.Create:
                materialDef = DefManager.Defs.Value<MaterialDef>(MaterialDefName);
                material = new Material(Id, materialDef, Amount, Item.ItemState.None)
                {
                    Health = Health
                };

                break;
            case ISaver.Phase.Link:
                // Inventory inventory = (Inventory)IdManager.GetThingById(InventoryId);
                // inventory.AddEntireItem(material, false);
                break;
        }
    }
}