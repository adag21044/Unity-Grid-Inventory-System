using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] ItemGrid selectedItemGrid;

    private void Update()
    {
        if(selectedItemGrid == null) return;

        Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        
    }
}
