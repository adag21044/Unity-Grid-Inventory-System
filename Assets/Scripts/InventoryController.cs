using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem; 
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    private void Update()
    {

        ItemIconDrag();


        if(Input.GetKeyDown(KeyCode.Q)) CreateRandomItem();

        // Ensure selectedItemGrid is not null before using it
        if (selectedItemGrid == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int SelectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[SelectedItemID]);
    }

    // Function to handle dragging the item icon
    public void ItemIconDrag()
    {
        // Update the rectTransform's position if it's not null
        if (rectTransform != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    // Method to handle mouse click actions
    private void LeftMouseButtonPress()
    {
        // Get the tile position on the grid based on the mouse position
        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        // If no item is selected, attempt to pick one up
        if (selectedItem == null)
        {
            TryPickUpItem(tileGridPosition);
        }
        else
        {
            PlaceSelectedItem(tileGridPosition);
        }
    }

    // Function to try to pick up an item from the grid
    private void TryPickUpItem(Vector2Int tileGridPosition)
    {
        // Pick up the item from the grid, if available
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        // If an item was picked up, set its RectTransform
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    // Function to place the selected item on the grid
    private void PlaceSelectedItem(Vector2Int tileGridPosition)
    {
        // Place the selected item on the grid at the clicked position
        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);

        // Reset the selected item and rectTransform after placing
        selectedItem = null;
        rectTransform = null;
    }
}
