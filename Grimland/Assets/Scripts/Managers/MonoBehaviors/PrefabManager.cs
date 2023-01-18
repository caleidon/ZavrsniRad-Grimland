using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; }

    public UnityEngine.Material Material;
    public GameObject Creature;

    public Transform itemWorldPrefab;

    private void Awake()
    {
        Instance = this;
    }
}