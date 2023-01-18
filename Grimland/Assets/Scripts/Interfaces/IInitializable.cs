public interface IInitializable
{
    public string Name { get; set; }
    public object CreateInstance();
}