using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int ID;
    [SerializeField] private string Name;
    [SerializeField] private string ItemText;
    [SerializeField] private int Have;
    [SerializeField] private Sprite ItemImage;
}
