using JetBrains.Annotations;
using UnityEngine;

public class FoodDef : ItemDef
{
    public string Extra { get; set; }

    [LoadedByYaml(YamlTag = "!Food")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public float MoveSpeedMultiplier { get; set; }
        public int MaxStackSize { get; set; }
        public int MaxHealth { get; set; }
        public string Sprite { get; set; }
        public string Extra { get; set; }

        public object CreateInstance()
        {
            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            tile.sprite = DefManager.Sprites[Sprite];

            FoodDef itemDef = new FoodDef
            {
                Name = Name,
                DisplayName = DisplayName,
                MoveSpeedMultiplier = MoveSpeedMultiplier,
                MaxStackSize = MaxStackSize,
                MaxHealth = MaxHealth,
                Extra = Extra,
                Tile = tile
            };

            return itemDef;
        }
    }

    public override Item Create(int amount)
    {
        // TODO: these checks have to always be copied. Do them in one place.
        if (amount > MaxStackSize)
        {
            Debug.LogError($"Tried to create item {DisplayName} with amount greater than MaxStackSize.");
        }

        if (amount <= 0)
        {
            Debug.LogError($"Tried to create item {DisplayName} with amount smaller than or equal to zero.");
        }

        Food food = new Food(null, this, amount, Item.ItemState.None);
        return food;
    }
}

public class Food : Item
{
    public string Extra { get; set; }

    public Food([CanBeNull] string id, FoodDef foodDef, int amount, ItemState state) : base(id, foodDef, amount, state)
    {
        Health = foodDef.MaxHealth;
        Extra = foodDef.Extra;
    }

    public override Item MakeCopy()
    {
        Food newFood = new Food(null, (FoodDef)ThingDef, Amount, State)
        {
            Health = Health,
            Extra = Extra
        };

        return newFood;
    }

    public override ISaver GetSaver()
    {
        return new FoodSaver(this);
    }

    // TODO: remvoe fromr egionthings
}

public class FoodSaver : ISaver
{
    public string Id { get; set; }
    public string FoodDefName { get; set; }
    public int Health { get; set; }
    public int Amount { get; set; }
    public string Extra { get; set; }
    public string InventoryId { get; set; }

    public FoodSaver(Food food)
    {
        Id = food.Id;
        FoodDefName = food.ThingDef.Name;
        Health = food.Health;
        Amount = food.Amount;
        Extra = food.Extra;
        // InventoryId = food.Inventory.Id;
    }

    public FoodSaver() { }

    private FoodDef foodDef;
    private Food food;

    public void Load(ISaver.Phase phase)
    {
        switch (phase)
        {
            case ISaver.Phase.Create:
                foodDef = DefManager.Defs.Value<FoodDef>(FoodDefName);
                food = new Food(Id, foodDef, Amount, Item.ItemState.None)
                {
                    Health = Health,
                    Extra = Extra
                };
                break;

            case ISaver.Phase.Link:
                // Inventory inventory = (Inventory)IdManager.GetThingById(InventoryId);
                // inventory.AddEntireItem(food, false);
                break;
        }
    }
}