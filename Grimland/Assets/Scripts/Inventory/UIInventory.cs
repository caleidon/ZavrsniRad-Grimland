// using System;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UIInventory : MonoBehaviour
// {
//     private Inventory inventory;
//     [SerializeField] private Transform itemSlotContainer;
//     [SerializeField] private Transform itemSlotTemplate;
//
//     public void SetInventory(Inventory inv)
//     {
//         inventory = inv;
//         inventory.OnItemListChanged += InventoryOnOnItemListChanged;
//         RefreshInventoryItems();
//     }
//
//     private void InventoryOnOnItemListChanged(object sender, EventArgs e)
//     {
//         RefreshInventoryItems();
//     }
//
//     private void RefreshInventoryItems()
//     {
//         foreach (Transform item in itemSlotContainer)
//         {
//             if (item != itemSlotTemplate) Destroy(item.gameObject);
//         }
//
//         foreach (ItemDef item in inventory.Items)
//         {
//             Transform newItem = Instantiate(itemSlotTemplate, itemSlotContainer);
//             RectTransform itemSlotRectTransform = newItem.GetComponent<RectTransform>();
//             itemSlotRectTransform.gameObject.SetActive(true);
//
//             Image itemImage = newItem.GetComponent<Image>();
//             itemImage.sprite = item.sprite;
//
//             TextMeshProUGUI uiText = itemSlotRectTransform.Find("ItemAmountText").GetComponent<TextMeshProUGUI>();
//             uiText.text = item.IsStackable ? item.Amount.ToString() : "";
//         }
//     }
// }

