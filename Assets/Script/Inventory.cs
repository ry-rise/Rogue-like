using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image itemImage = null;
    [SerializeField] private Text itemName = null;
    [SerializeField] private Text itemValue = null;
    [SerializeField] private ItemDataBase itemDataBase = null;
    private ItemData itemData;
    private int itemNumber;
    void Start()
    {
        itemNumber = 0;
        SetInventory();
    }

    void Update()
    {
        //z:前へ
        if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (itemNumber > 0) itemNumber -= 1;
            SetInventory();
        }

        //x:次へ
        else if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            int max = itemDataBase.GetItemLists().Count - 1;
            if (itemNumber < max) itemNumber += 1;
            SetInventory();
        }
    }

    void SetInventory()
    {
        itemData = itemDataBase.GetItemLists()[itemNumber];
        itemImage.sprite = itemData.GetItemImage();
        itemName.text = itemData.GetItemName();
        itemValue.text = itemData.GetItemValue();
    }
}
