using System;

[AttributeUsage(AttributeTargets.Class)]
public class LoadedByYaml : Attribute
{
    public string YamlTag { get; set; }
}