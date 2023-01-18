public interface ISaver
{
    public enum Phase
    {
        Create,
        Link,
        Instantiate
    }

    public void Load(Phase phase);
}