using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using YamlDotNet.Serialization;

public static class DefManager
{
    public static readonly GenericDictionary Defs = new GenericDictionary();
    public static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    public static readonly Dictionary<string, ExternalBehavior> Behaviors = new Dictionary<string, ExternalBehavior>();
    public static readonly Dictionary<string, ConnectableRuleTile> RuleTiles = new Dictionary<string, ConnectableRuleTile>();

    public static async void LoadDefs()
    {
        LoadingManager.SetLoading(true, LoadingManager.LoadingType.Defs);
        await LoadSprites();
        await LoadYamlFiles();
        await LoadBehaviors();
        LoadingManager.SetLoading(false);
    }

    private static async Task<T[]> LoadAssetsByLabel<T>(string assetLabel)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(assetLabel, typeof(T)).Task;
        List<Task<T>> tasks = new List<Task<T>>();

        foreach (var location in locations)
        {
            tasks.Add(Addressables.LoadAssetAsync<T>(location).Task);
        }

        return await Task.WhenAll(tasks);
    }

    private static async Task LoadBehaviors()
    {
        var behaviors = await LoadAssetsByLabel<ExternalBehavior>("behaviors");

        foreach (var behavior in behaviors)
        {
            Behaviors.Add(behavior.name, behavior);
        }
    }

    private static async Task LoadSprites()
    {
        var loadedSprites = await LoadAssetsByLabel<Sprite>("sprites");

        foreach (var sprite in loadedSprites)
        {
            Sprites.Add(sprite.name, sprite);
        }
    }

    private static async Task LoadYamlFiles()
    {
        var textAssets = await LoadAssetsByLabel<TextAsset>("yaml");

        var deserializerBuilder = new DeserializerBuilder();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(LoadedByYaml), true);
                if (attributes.Length > 0)
                {
                    deserializerBuilder = deserializerBuilder.WithTagMapping(((LoadedByYaml)attributes[0]).YamlTag, type);
                }
            }
        }

        var deserializer = deserializerBuilder.Build();

        foreach (var textAsset in textAssets)
        {
            var def = deserializer.Deserialize<IInitializable>(textAsset.text);
            Defs.Add(def.Name, def.CreateInstance());
        }
    }


    public static void ReloadDefs()
    {
        if (Directory.Exists(Caching.defaultCache.path))
            Directory.Delete(Caching.defaultCache.path, true);
        Caching.ClearCache();
        Defs.Clear();
        Sprites.Clear();
        LoadDefs();
    }

    public static BaseTileDef GetBaseTileByHeight(float height)
    {
        BaseTileDef bestTileDefType = null;

        foreach (var baseTile in Defs.Values<BaseTileDef>())
        {
            if (baseTile.GenHeight < 0 || baseTile.GenHeight < height)
            {
                continue;
            }

            if (bestTileDefType == null || baseTile.GenHeight < bestTileDefType.GenHeight)
            {
                bestTileDefType = baseTile;
            }
        }

        return bestTileDefType;
    }
}