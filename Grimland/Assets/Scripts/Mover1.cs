using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover1 : MonoBehaviour
{
    private AIPath astar;

    private void Awake()
    {
        astar = GetComponent<AIPath>();
    }


    public void Update()
    {
        if (Keyboard.current[Key.Digit6].wasPressedThisFrame)
        {
            Vector3Int startingNode = Map.GridPosFromMousePos();

            astar.destination = startingNode;
            astar.SearchPath();
        }
    }
}