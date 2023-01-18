using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image pausedImage;
    [SerializeField] private GameObject gameUI;

    private void Awake()
    {
        Instance = this;

        TickManager.OnPause += SetPausedImage;
    }

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    // TODO: ui objects for selecting tiles have huge hitbox blocking placement (REWORK ENTIRE UI)

    public void SetGameUI(bool isEnabled)
    {
        gameUI.SetActive(isEnabled);
    }

    public void SetPausedImage(bool isEnabled)
    {
        pausedImage.gameObject.SetActive(isEnabled);
    }

    public void SetAddZone(bool isEnabled)
    {
        SelectionManager.Instance.ChangeMode(SelectionManager.Mode.BoxSelection);
        SelectionManager.Instance.ChangeAction(SelectionManager.Action.AddZone);
    }

    public void SetRemoveZone(bool isEnabled)
    {
        SelectionManager.Instance.ChangeMode(SelectionManager.Mode.BoxSelection);
        SelectionManager.Instance.ChangeAction(SelectionManager.Action.RemoveZone);
    }

    private void OnDisable()
    {
        TickManager.OnPause -= SetPausedImage;
    }

    // CameraController.OnZoomChange += OnZoomChange;

    // private readonly Dictionary<Vector3Int, GameObject>
    //     itemDisplayDictionary = new Dictionary<Vector3Int, GameObject>();
    //
    // private bool itemCountVisibility;
    //
    // [SerializeField] private GameObject textHolder;
    // [SerializeField] private GameObject dynamicObjectParent;

    /*private void OnZoomChange(object sender, CameraController.OnZoomChangeArgs e)
    {
        if (e.ZoomLevel <= 11f)
        {
            itemCountVisibility = true;
            ChangeAllItemCountVisibility(itemCountVisibility);
        }
        else
        {
            itemCountVisibility = false;
            ChangeAllItemCountVisibility(itemCountVisibility);
        }
    }*/

    /*private void ChangeAllItemCountVisibility(bool visibility)
    {
        if (itemDisplayDictionary.Count <= 0)
        {
            return;
        }

        foreach (GameObject itemDisplay in itemDisplayDictionary.Values)
        {
            GameObject itemCount = itemDisplay.transform.GetChild(0).gameObject;
            itemCount.SetActive(visibility);
        }
    }

    private void ChangeItemCountVisibility(Vector3Int position, bool visibility)
    {
        GameObject itemCount = itemDisplayDictionary[position].transform.GetChild(0).gameObject;
        itemCount.SetActive(visibility);
    }

    public void LoadItemDisplays(Map map)
    {
        ClearItemDisplayInventory();

        foreach (KeyValuePair<Vector3Int, TileData> tileData in map.PositionData)
        {
            if (tileData.Value.Inventory.InventoryData.Count > 0)
            {
                foreach (KeyValuePair<ItemObject, int> kv in tileData.Value.Inventory.InventoryData)
                {
                    itemDisplayDictionary[tileData.Key] =
                        CreateItemDisplayObject(kv.Key, kv.Value, tileData.Key);
                    itemDisplayDictionary[tileData.Key].transform
                        .SetParent(GameObject.FindWithTag("Items").transform, false);
                }
            }
        }

        ChangeAllItemCountVisibility(itemCountVisibility);
    }

    public void CreateItemDisplay(ItemObject item, int amount, Vector3Int position)
    {
        itemDisplayDictionary[position] = CreateItemDisplayObject(item, amount, position);
        itemDisplayDictionary[position].transform.SetParent(GameObject.FindWithTag("Items").transform, false);
        ChangeItemCountVisibility(position, itemCountVisibility);
    }

    public void UpdateItemDisplayAmount(Vector3Int position, int newAmount)
    {
        itemDisplayDictionary[position].GetComponent<ItemDisplay>().SetItemCount(newAmount);
    }

    public void DestroyItemDisplay(Vector3Int position)
    {
        Destroy(itemDisplayDictionary[position]);
        itemDisplayDictionary.Remove(position);
    }

    public void ClearItemDisplayInventory()
    {
        itemDisplayDictionary.Clear();
    }

    private static GameObject CreateItemDisplayObject(ItemObject item, int amount, Vector3Int position)
    {
        GameObject itemDisplayObject = Instantiate(Instance.textHolder, position, Quaternion.identity);
        itemDisplayObject.name = $"{item.name} {position}";

        itemDisplayObject.GetComponent<ItemDisplay>().SetItemCount(amount);

        itemDisplayObject.GetComponent<SpriteRenderer>().sprite = item.Sprite;

        return itemDisplayObject;
    }

    public GameObject CreateSortingObject()
    {
        GameObject sortingObject = Instantiate(dynamicObjectParent, new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity);
        sortingObject.name = "Sorting Object";
        return sortingObject;
    }*/
}