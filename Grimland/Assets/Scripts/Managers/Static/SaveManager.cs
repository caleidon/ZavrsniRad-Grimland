using BehaviorDesigner.Runtime;

public static class SaveManager
{
    public static Map Map { get; set; }
    public static IdManagerSaver IdManagerSaver { get; set; }
    public static NodeManagerSaver NodeManagerSaver { get; set; }
    public static TickManagerSaver TickManagerSaver { get; set; }

    public static BehaviorManager.BehaviorTree Tree { get; set; }

    public static void Save(string saveName)
    {
        Map = Map.Instance;
        IdManagerSaver = new IdManagerSaver();
        TickManagerSaver = new TickManagerSaver();
        NodeManagerSaver = new NodeManagerSaver();

        // Things saved first here will appear at the bottom of the save file
        // Use this wise information wisely, young padawan
        ES3.Save("NodeManagerSaver", NodeManagerSaver, $"Saves/{saveName}.es3");
        ES3.Save("IdManagerSaver", IdManagerSaver, $"Saves/{saveName}.es3");
        ES3.Save("TickManagerSaver", TickManagerSaver, $"Saves/{saveName}.es3");
        ES3.Save("Map", Map, $"Saves/{saveName}.es3");
    }

    public static Map Load(string saveName)
    {
        Map = ES3.Load<Map>("Map", $"Saves/{saveName}.es3");
        TickManagerSaver = ES3.Load<TickManagerSaver>("TickManagerSaver", $"Saves/{saveName}.es3");
        IdManagerSaver = ES3.Load<IdManagerSaver>("IdManagerSaver", $"Saves/{saveName}.es3");
        NodeManagerSaver = ES3.Load<NodeManagerSaver>("NodeManagerSaver", $"Saves/{saveName}.es3");
        return Map;
    }

    public static void SaveTree(BehaviorManager.BehaviorTree tree)
    {
        Tree = tree;
        ES3.Save("Tree", Tree, "Saves/tree.es3");
    }

    public static BehaviorManager.BehaviorTree LoadTree()
    {
        Tree = ES3.Load<BehaviorManager.BehaviorTree>("Tree", "Saves/tree.es3");
        return Tree;
    }
}