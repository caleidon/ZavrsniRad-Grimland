using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [SerializeField] private Transform selectionAreaTransform;

    public enum Mode
    {
        EntitySelection,
        BoxSelection,
        LineSelection,
        None
    }

    // TODO: add limits of 100x100 to box selection etc

    public enum Action
    {
        AddZone,
        RemoveZone,
        None
    }

    // TODO: add selectionPriorities only ON CLICK (only when 1 tiles detected)
    // do this in a way that only the final tile is selected

    public Mode currentMode;
    public Action currentAction;
    private Vector3 startNode;
    private Vector3 endNode;

    public static HashSet<Vector3Int> selectedNodes = new HashSet<Vector3Int>();
    // private static readonly HashSet<IEntity> selectedEntities = new HashSet<IEntity>();

    public Tile selectedTile;
    public bool isDoorSelected;

    private void Awake()
    {
        Instance = this;

        currentMode = Mode.None;
    }

    public void OnLmbClick()
    {
        if (UIManager.IsMouseOverUI() || currentMode == Mode.None)
        {
            return;
        }

        if (currentMode == Mode.BoxSelection)
        {
            startNode = Map.GridPosFromMousePos();
        }

        // if (currentMode == Mode.EntitySelection)
        // {
        //     // selectedEntities.Clear();
        //     selectionAreaTransform.gameObject.SetActive(true);
        // }

        // startNode = Utilities.MouseWorldPosition();
    }

    public void OnLmbHold()
    {
        if (UIManager.IsMouseOverUI() || currentMode == Mode.None)
        {
            return;
        }

        switch (currentMode)
        {
            // case Mode.EntitySelection:
            //     HandleEntitySelection();
            //     break;
            case Mode.BoxSelection:
                HandleBoxSelection();
                break;
            // case Mode.LinePlacement:
            //     HandleLineSelection();
            //     break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnLmbRelease()
    {
        if (UIManager.IsMouseOverUI() || currentMode == Mode.None)
        {
            return;
        }

        if (selectedNodes.Count > 0)
        {
            switch (currentAction)
            {
                case Action.AddZone:
                    ZoneManager.MakeNewZone(selectedNodes);
                    break;
                case Action.RemoveZone:
                    ZoneManager.RemoveZone(selectedNodes);
                    break;
                case Action.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        selectedNodes.Clear();
    }


    public void OnRmbClick()
    {
        if (UIManager.IsMouseOverUI() || currentMode == Mode.None)
        {
            return;
        }

        switch (currentAction)
        {
            case Action.AddZone:
                currentMode = Mode.None;
                currentAction = Action.None;
                break;
            case Action.RemoveZone:
                currentMode = Mode.None;
                currentAction = Action.None;
                break;
            case Action.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        selectedNodes.Clear();


        // if (selectedEntities.Count > 0)
        // {
        //     foreach (var entity in selectedEntities)
        //     {
        //         if (entity is Creature creature)
        //         {
        //             // TODO: tell creatures to move here
        //         }
        //     }
        // }
    }


    private void HandleBoxSelection()
    {
        Vector3 newEndNode = Map.GridPosFromMousePos();
        if (newEndNode == endNode)
        {
            return;
        }

        selectedNodes.Clear();
        endNode = newEndNode;

        Vector3 lowerLeftTile = new Vector3(Mathf.Min(startNode.x, endNode.x), Mathf.Min(startNode.y, endNode.y));
        Vector3 upperRightTile = new Vector3(Mathf.Max(startNode.x, endNode.x), Mathf.Max(startNode.y, endNode.y));

        SelectTilesWithinNodes(lowerLeftTile, upperRightTile);
    }

    private static void SelectTilesWithinNodes(Vector3 node1, Vector3 node2)
    {
        for (float x = node1.x; x <= node2.x; x++)
        {
            for (float y = node1.y; y <= node2.y; y++)
            {
                Vector3Int newNode = Map.GridPosFromWorldPos(new Vector2(x, y));
                if (Map.Contains(newNode))
                {
                    selectedNodes.Add(newNode);
                }
            }
        }
    }

    public void ChangeMode(Mode mode)
    {
        switch (mode)
        {
            // case Mode.EntitySelection:
            //     currentMode = Mode.EntitySelection;
            //     break;

            case Mode.BoxSelection:
                currentMode = Mode.BoxSelection;
                break;

            case Mode.LineSelection:
                currentMode = Mode.LineSelection;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public void ChangeAction(Action action)
    {
        switch (action)
        {
            // case Mode.EntitySelection:
            //     currentMode = Mode.EntitySelection;
            //     break;

            case Action.AddZone:
                currentAction = Action.AddZone;
                break;

            case Action.RemoveZone:
                currentAction = Action.RemoveZone;
                break;

            case Action.None:
                currentAction = Action.None;
                break;

            // case Mode.LineSelection:
            //     currentMode = Mode.LineSelection;
            //     break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    // private void HandleEntitySelection()
    // {
    //     endNode = Utilities.MouseWorldPosition();
    //
    //     Vector3 lowerLeftPos = new Vector3(Mathf.Min(startNode.x, endNode.x), Mathf.Min(startNode.y, endNode.y));
    //     Vector3 upperRightPos = new Vector3(Mathf.Max(startNode.x, endNode.x), Mathf.Max(startNode.y, endNode.y));
    //
    //     selectionAreaTransform.position = lowerLeftPos;
    //     selectionAreaTransform.localScale = upperRightPos - lowerLeftPos;
    //
    //     SelectTilesWithinNodes(lowerLeftPos, upperRightPos);
    // }

    // private void HandleLineSelection()
    // {
    //     // TODO: Implement LineBuilding mode
    //      if (Mouse.current.leftButton.wasPressedThisFrame)
    //      {
    //          selectionAreaTransform.gameObject.SetActive(true);
    //          startNode = Utilities.MouseWorldNode();
    //      }
    //
    //      if (Mouse.current.leftButton.wasReleasedThisFrame)
    //      {
    //          selectionAreaTransform.gameObject.SetActive(false);
    //
    //          Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startNode, Utilities.MouseWorldNode());
    //
    //          selectedEntities.Clear();
    //
    //          if (collider2DArray.Length > 0)
    //          {
    //              foreach (var collider in collider2DArray)
    //              {
    //                  ISelectable selectable = collider.GetComponent<ISelectable>();
    //                  if (selectable != null)
    //                  {
    //                      selectedEntities.Add(selectable);
    //                      selectable.OnSelect();
    //                  }
    //              }
    //
    //              return;
    //          }
    //
    //          Grid grid = Map.Instance.Grid.GetComponent<Grid>();
    //          Vector3Int bottomLeftCell = grid.WorldToCell(lowerLeft);
    //          Vector3Int topRightCell = grid.WorldToCell(upperRight);
    //
    //          Vector3Int min = Vector3Int.Min(bottomLeftCell, topRightCell);
    //          Vector3Int max = Vector3Int.Max(bottomLeftCell, topRightCell);
    //          Vector3Int size = max - min + Vector3Int.one;
    //
    //          BoundsInt bounds = new BoundsInt(min, size);
    //          TileBase[] vegetationTiles = Map.Instance.VegetationTilemap.GetTilesBlock(bounds);
    //
    //          foreach (var tile in vegetationTiles)
    //          {
    //              ISelectable selectableTile = tile as ISelectable;
    //              if (selectableTile != null)
    //              {
    //                  selectedEntities.Add(selectableTile);
    //                  Debug.Log($"adding {selectableTile}");
    //                  selectableTile.OnSelect();
    //              }
    //          }
    //      }
    //
    //     if (isPlacingTiles)
    //     {
    //         if (selectDirect == SelectDirect.None && selectedNodes.Count == 2)
    //         {
    //             Vector3Int selectionDirection = selectedNodes[1] - selectedNodes[0];
    //
    //             // Abort if selection started diagonally
    //             if (Mathf.Abs(selectionDirection.x) + Mathf.Abs(selectionDirection.y) > 1)
    //             {
    //                 ResetSelection();
    //                 selectedNodes.Clear();
    //                 return;
    //             }
    //
    //             if (Mathf.Abs(selectionDirection.x) == 1)
    //             {
    //                 selectDirect = SelectDirect.Horizontal;
    //                 fixedY = selectedNodes[0].y;
    //             }
    //             else
    //             {
    //                 selectDirect = SelectDirect.Vertical;
    //                 fixedX = selectedNodes[0].x;
    //             }
    //         }
    //
    //         Vector3Int gridNode = Utilities.GridPosFromMousePos();
    //         if (Map.Instance.IsInsideMap(gridNode))
    //         {
    //             switch (selectDirect)
    //             {
    //                 case SelectDirect.None:
    //                     if (!selectedNodes.Contains(gridNode)) { selectedNodes.Add(gridNode); }
    //                     break;
    //                 case SelectDirect.Horizontal:
    //
    //                     if (gridNode.y == fixedY && !selectedNodes.Contains(gridNode)) { selectedNodes.Add(gridNode); }
    //                     else if (gridNode.y != fixedY && !selectedNodes.Contains(new Vector3Int(gridNode.x, fixedY, 0)))
    //                     {
    //                         selectedNodes.Add(new Vector3Int(gridNode.x, fixedY, 0));
    //                     }
    //
    //                     break;
    //                 case SelectDirect.Vertical:
    //                     if (gridNode.x == fixedX && !selectedNodes.Contains(gridNode)) { selectedNodes.Add(gridNode); }
    //                     else if (gridNode.x != fixedX && !selectedNodes.Contains(new Vector3Int(fixedX, gridNode.y, 0)))
    //                     {
    //                         selectedNodes.Add(new Vector3Int(fixedX, gridNode.y, 0));
    //                     }
    //                     break;
    //             }
    //         }
    //     }
    // }


    // private bool SelectCreaturesByCollider()
    // {
    //     Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startNode, endNode);
    //
    //     if (collider2DArray.Length > 0)
    //     {
    //         foreach (var colliderHit in collider2DArray)
    //         {
    //             Creature creature = colliderHit.GetComponent<Creature>();
    //             if (creature != null)
    //             {
    //                 selectedEntities.Add(creature);
    //                 Debug.Log(creature.gameObject.name);
    //             }
    //         }
    //     }
    //
    //     return selectedEntities.Count > 0;
    // }

    // private void SelectEntitiesByTiles()
    // {
    //     Type priorityType = null;
    //
    //     foreach (var node in selectedNodes)
    //     {
    //         IEntity entity = (IEntity) Map.Instance.nodeData[node.x, node.y].GetThings()[0];
    //
    //         if (entity != null) // && entity.IsSelectable
    //         {
    //             Type entityType = entity.GetType();
    //
    //             if (priorityType == null)
    //             {
    //                 priorityType = entityType;
    //             }
    //             else
    //             {
    //                 foreach (var type in selectionPriorities)
    //                 {
    //                     if (priorityType == type || entityType == type)
    //                     {
    //                         priorityType = type;
    //                         break;
    //                     }
    //                 }
    //             }
    //
    //             if (selectionPriorities.IndexOf(entityType) >= selectionPriorities.IndexOf(priorityType))
    //             {
    //                 selectedEntities.Add(entity);
    //             }
    //         }
    //     }
    //
    //     foreach (var entity in selectedEntities.ToList())
    //     {
    //         if (entity.GetType() != priorityType)
    //         {
    //             selectedEntities.Remove(entity);
    //         }
    //     }
    // }


    // public void MarkPlant()
    // {
    //     Vector3Int selectedNode = Map.GridPosFromMousePos();
    //     selectedNodes.Add(selectedNode);
    //     SelectEntitiesByTiles();
    //
    //     if (selectedEntities.Count > 0 && selectedEntities.First() is VegetationTile)
    //     {
    //         VegetationTile selectedTree = (VegetationTile) selectedEntities.First();
    //         //JobManager.EnqueueJob(new KeyValuePair<string, JobManager.Jobs>(selectedTree.ID, JobManager.Jobs.Chop));
    //         Debug.Log($"Marked {selectedTree} for cutting at {selectedNode}");
    //     }
    //
    //     selectedNodes.Clear();
    // }
}