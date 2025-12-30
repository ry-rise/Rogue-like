using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    [SerializeField] private ItemDataBase itemDataBase;
    private Dictionary<ItemData, int> numItem;
    void Awake()
    {
        // Singleton（任意。不要なら消してOK）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        numItem = new Dictionary<ItemData, int>();

        if (itemDataBase == null)
        {
            Debug.LogError("[ItemManager] ItemDataBase が Inspector で設定されていません");
            enabled = false; // ★これ以上動かさない
            return;
        }

        var list = itemDataBase.GetItemLists();
        if (list == null)
        {
            Debug.LogError("[ItemManager] ItemDataBase.GetItemLists() が null");
            enabled = false;
            return;
        }

        foreach (var item in list)
        {
            if (item == null) continue;
            numItem[item] = 0; // 初期は0の方が自然（今後増やす）
        }
    }
    
    public void AddItem(ItemData item, int amount = 1)
    {
        if (!enabled) return;
        if (item == null) return;
        if (!numItem.ContainsKey(item)) numItem[item] = 0;
        numItem[item] += amount;
    }

    public int GetCount(ItemData item)
    {
        if (!enabled) return 0;
        if (item == null) return 0;
        return numItem.TryGetValue(item, out var v) ? v : 0;
    }
}
