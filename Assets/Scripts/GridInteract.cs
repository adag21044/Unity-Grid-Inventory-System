using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    ItemGrid itemGrid;
    HighlightItem highlightItem;

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        itemGrid = GetComponent<ItemGrid>();
        highlightItem = GetComponent<HighlightItem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = itemGrid;

        // Highlight the item if available
        if (highlightItem != null)
        {
            highlightItem.Highlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = null;

        // Remove the highlight from the item
        if (highlightItem != null)
        {
            highlightItem.RemoveHighlight();
        }
    }
}
