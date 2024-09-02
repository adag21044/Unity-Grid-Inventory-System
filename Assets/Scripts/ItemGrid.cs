using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32f;
    public const float tileSizeHeight = 32f;

    InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform;

    [SerializeField] int gridSizeWidth = 20;
    [SerializeField] int gridSizeHeight = 10;

    

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
  
    }

    public InventoryItem PickUpItem(int posX, int posY)
    {
        InventoryItem toReturn = inventoryItemSlot[posX, posY];

        if(toReturn == null) return null;
        

        for(int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for(int iy = 0; iy < toReturn.itemData.height; iy++)
            {
                inventoryItemSlot[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }

        
        return toReturn;
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        // Sınırları kontrol et
        if (BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height) == false) 
            return false;

        // Üst üste binme durumunu kontrol et
        if (OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem) == false) 
        {
            overlapItem = null;
            return false;
        }

        // Eğer overlapItem null değilse, eski referansları temizle
        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        // Grid'e yerleştir
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for (int y = 0; y < inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        // Nesnenin grid üzerindeki pozisyonunu hesapla
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.height / 2);

        rectTransform.localPosition = position;

        return true;
    }


    bool PositionCheck(int posX, int posY)
    {
        if(posX < 0 || posY < 0)
        {
            return false;
        }

        if(posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(PositionCheck(posX, posY) == false) return false;

        posX += width - 1;
        posY += height - 1;

        if(PositionCheck(posX, posY) == false) return false;

        return true;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Eğer gridde bu pozisyonda başka bir item varsa
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null) 
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false; // Farklı bir item ile çakışma var
                        }
                    }
                }
            }
        }

        return true; // Çakışma yok veya sadece aynı item ile çakışma var
    }


    // Function to clean grid references for the given item
    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.height; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    

}
