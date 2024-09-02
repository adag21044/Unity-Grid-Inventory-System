using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the tile position on the grid based on the mouse position
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            // Check if an item is already selected
            if (selectedItem == null)
            {
                // Pick up the item from the grid, if available
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            }
            else
            {
                // Place the selected item on the grid at the clicked position
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                
                // Reset the selected item after placing
                selectedItem = null;
            }
        
        }
    }
}
