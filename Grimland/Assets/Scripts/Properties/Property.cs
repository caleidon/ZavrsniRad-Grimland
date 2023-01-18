public abstract class Property
{
    public object Parent { get; set; }
    public virtual void Apply() { }
    public virtual void Discard(object thing) { }

    public abstract class Initializer
    {
        public object Parent { get; set; }
        public abstract Property CreateInstance(object parent);
    }
}