using BehaviorDesigner.Runtime;
using UnityEngine;

public class TestBehaviorTreeSerialize : MonoBehaviour
{
    public string Json;

    public BehaviorTree behaviorTree;

    // private void Awake()
    // {
    //     behaviorTree = GetComponent<BehaviorTree>();
    // }
    //
    // private void Start()
    // {
    //     var tree = SaveManager.LoadTree();
    //     BehaviorManager.instance.BehaviorTrees[0] = tree;
    // }

    [ContextMenu("Save")]
    private void Save()
    {
        TickManager.PauseGame();
        BehaviorManager.BehaviorTree tree = BehaviorManager.instance.BehaviorTrees[0];
        SaveManager.SaveTree(tree);
        TickManager.ResumeGame();
    }

    [ContextMenu("Load")]
    private void Load()
    {
        var tree = SaveManager.LoadTree();
        // BehaviorManager.instance.BehaviorTrees[0] = null;
        BehaviorManager.instance.BehaviorTrees[0] = tree;
        // BehaviorManager.instance.BehaviorTrees.Add(tree);
        // Debug.Log("Loading");
        // Debug.Log("Loaded");
        // behaviorTree = tree;
        // var source = JsonUtility.FromJson<BehaviorSource>(Json);
        // behaviorTree.enabled = false;
        // source.Initialize(behaviorTree);
        // if (source.CheckForSerialization(true))
        // {
        //     behaviorTree.SetBehaviorSource(source);
        // }
        //
        // behaviorTree.enabled = true;
    }
}