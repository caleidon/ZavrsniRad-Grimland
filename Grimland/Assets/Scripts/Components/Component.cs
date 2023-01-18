using System.Collections.Generic;

public abstract class Component
{
    public object Parent { get; set; }
    public HashSet<string> Tags { get; set; }
    public Dictionary<string, Property> Properties { get; set; } = new Dictionary<string, Property>();

    public abstract class Initializer
    {
        public object Parent { get; set; }
        public HashSet<string> Tags { get; set; }
        public List<Property.Initializer> Properties { get; set; }

        public abstract Component CreateInstance(object parent);
    }

    public virtual void Tick() { }

    public virtual void MediumTick() { }

    public virtual void LongTick() { }

    public virtual void MegaTick() { }
}