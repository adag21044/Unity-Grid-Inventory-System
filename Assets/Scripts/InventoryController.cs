using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;
    RectTransform rectTransform;

    private void Update()
    {
        // Ensure selectedItemGrid is not null before using it
        if (selectedItemGrid == null) return;

        // Update the rectTransform's position if it's not null
        if (rectTransform != null)
        {
            rectTransform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Get the tile position on the grid based on the mouse position
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            // If no item is selected, attempt to pick one up
            if (selectedItem == null)
            {
                // Pick up the item from the grid, if available
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

                // If an item was picked up, set its RectTransform
                if (selectedItem != null)
                {
                    rectTransform = selectedItem.GetComponent<RectTransform>();
                }
            }
            else
            {
                // Place the selected item on the grid at the clicked position
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                
                // Reset the selected item and rectTransform after placing
                selectedItem = null;
                rectTransform = null;
            }
        }
    }
}
