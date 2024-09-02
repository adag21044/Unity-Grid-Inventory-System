using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;
    InventoryItem overlapItem; 
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    private void Update()
    {
        ItemIconDrag();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        // Ensure selectedItemGrid is not null before using it
        if (selectedItemGrid == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        // Instantiate a new inventory item and set its parent to the canvas
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int SelectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[SelectedItemID]);

        // Set the position of the item to the mouse position
        rectTransform.position = Input.mousePosition;
    }

    // Function to handle dragging the item icon
    public void ItemIconDrag()
    {
        // Update the rectTransform's position if it's not null
        if (rectTransform != null && selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    // Method to handle mouse click actions
    private void LeftMouseButtonPress()
    {
        Vector2 position = Input.mousePosition;

        // Debug log to track the state of selectedItem
        Debug.Log($"Selected Item: {(selectedItem != null ? selectedItem.name : "None")}");

        // Ensure selectedItemGrid is not null before proceeding
        if (selectedItemGrid == null)
        {
            Debug.LogError("SelectedItemGrid is null! Please ensure it is assigned correctly in the Inspector.");
            return;
        }

        // Adjust position if selectedItem is not null
        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * ItemGrid.tileSizeHeight / 2;
        }

        // Get the tile position on the grid based on the mouse position
        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(position);

        // Debug log to track the tile position
        Debug.Log($"Mouse Position: {position}, Tile Grid Position: {tileGridPosition}");

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
        Debug.Log($"Trying to pick up item at position: {tileGridPosition}");

        // Pick up the item from the grid, if available
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        // If an item was picked up, set its RectTransform
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            Debug.Log("Item picked up successfully.");
        }
        else
        {
            Debug.LogWarning("No item was picked up.");
        }
    }

    // Function to place the selected item on the grid
    private void PlaceSelectedItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if(complete)
        {
            // Reset the selected item and rectTransform after placing
            selectedItem = null;
            rectTransform = null;

            if(overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }
}
