using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase;
    private Dictionary<ItemData, int> NumItem;
    void Awake()
    {
        NumItem = new Dictionary<ItemData, int>();
    }
    void Start()
    {
        if (itemDataBase == null)
        {
            Debug.LogError("ItemDataBase が Inspector で設定されていません");
        }
        var list = itemDataBase.GetItemLists();
        
        for (int i = 0; i < list.Count; i += 1)
        {
            if(list[i] == null) continue;
            NumItem[list[i]] = 1;
        }
    }
}
