using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private AIPath aipath;

    private void Awake()
    {
        aipath = GetComponent<AIPath>();
    }


    public void Update()
    {
        if (Keyboard.current[Key.Digit5].wasPressedThisFrame)
        {
            Vector3Int startingNode = Map.GridPosFromMousePos();

            aipath.destination = startingNode;
            aipath.SearchPath();
        }
    }
}