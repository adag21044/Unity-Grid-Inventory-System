using UnityEngine;

[CreateAssetMenu(menuName = "Unity Grid Inventory System/ItemData")]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;
}