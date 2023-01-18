public static class Settings
{
    public const int REGION_SIZE = 12;
    //public const int TicksPerDay = 3000;
    //public const int DaysPerSeason = 15;
    //public const int SeasonsPerYear = 4;

    public enum LayerOrder
    {
        BaseTilemap,
        FloorTilemap,
        VegetationTilemap,
        BlueprintTilemap,
        BuildingTilemap,
        ItemTilemap,
        Creature
    }
}