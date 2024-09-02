using UnityEngine;
using UnityEngine.UI;

public class HighlightItem : MonoBehaviour
{
    private Image itemImage;
    private Color originalColor;
    public Color highlightColor = Color.yellow; // Customize this color as needed

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        if (itemImage != null)
        {
            originalColor = itemImage.color;
        }
    }

    public void Highlight()
    {
        if (itemImage != null)
        {
            itemImage.color = highlightColor;
        }
    }

    public void RemoveHighlight()
    {
        if (itemImage != null)
        {
            itemImage.color = originalColor;
        }
    }
}
