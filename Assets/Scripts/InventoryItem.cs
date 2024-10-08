using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    public int onGridPositionX;
    public int onGridPositionY;

    private void Awake()
    {
        // Add the HighlightItem component if it's not already attached
        if (GetComponent<HighlightItem>() == null)
        {
            gameObject.AddComponent<HighlightItem>();
        }
    }

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
