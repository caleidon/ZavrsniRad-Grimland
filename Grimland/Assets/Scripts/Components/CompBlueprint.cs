using System.Collections.Generic;

public class CompBlueprint : Component
{
    public List<Dictionary<string, string>> RequiredMaterials { get; set; }

    [LoadedByYaml(YamlTag = "!Blueprint")]
    public new class Initializer : Component.Initializer
    {
        public List<Dictionary<string, string>> RequiredMaterials { get; set; }

        public override Component CreateInstance(object parent)
        {
            CompBlueprint compBlueprint = new CompBlueprint
            {
                Parent = parent,
                Tags = Tags,
                RequiredMaterials = RequiredMaterials
            };

            if (Properties != null)
            {
                foreach (var propertyInitializer in Properties)
                {
                    Property property = propertyInitializer.CreateInstance(compBlueprint);
                    compBlueprint.Properties.Add(property.GetType().Name, property);
                    property.Apply();
                }
            }

            return compBlueprint;
        }
    }
}