public interface ICanBeSaved
{
    public string Id { get; set; }
    public ISaver GetSaver();
}