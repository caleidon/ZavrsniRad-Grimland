using System.Collections.Generic;
using System.Linq;

public class GenericDictionary
{
    private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

    public void Add(string key, object value)
    {
        dictionary.Add(key, value);
    }

    public List<string> GetKeys()
    {
        return new List<string>(dictionary.Keys);
    }

    public T Value<T>(string key) where T : class
    {
        return dictionary[key] as T;
    }

    public List<T> Values<T>() where T : class
    {
        return dictionary.Values.OfType<T>().ToList();
    }

    public void Clear()
    {
        dictionary.Clear();
    }
}