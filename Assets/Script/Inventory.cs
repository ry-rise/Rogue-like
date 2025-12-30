using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InputKey;

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
        if (InputManager.GridInputKeyDown(KeyCode.Z))
        {
            if (itemNumber != 0)
            {
                itemNumber -= 1;
            }
            itemData = itemDataBase.GetItemLists()[itemNumber];
            SetInventory();
        }
        else if (InputManager.GridInputKeyDown(KeyCode.X))
        {
            if (itemNumber != 0)
			{
				itemNumber += 1;
			}
			itemData=itemDataBase.GetItemLists()[itemNumber];
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
