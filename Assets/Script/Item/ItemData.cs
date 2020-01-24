using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int ID;
    [SerializeField] private string ItemName;
    [SerializeField] private string ItemText;
    [SerializeField] private int ItemValue;
    [SerializeField] private Sprite ItemImage;
    public Sprite GetItemImage()
    {
        return ItemImage;
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemValue()
    {
        return ItemValue.ToString();
    }
}
