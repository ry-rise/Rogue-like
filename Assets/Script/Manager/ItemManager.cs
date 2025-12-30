using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour 
{
    [SerializeField]private ItemDataBase itemDataBase;
	private Dictionary<ItemData,int> NumItem=null;

    void Start()
    {
        if(itemDataBase.GetItemLists()[0]==null) Debug.Log("NULL!!!!!!!");
        var a=itemDataBase.GetItemLists();
        Debug.Log(NumItem);
        Debug.Log(a[0]);
        // foreach (var i in Enumerable.Range(0,itemDataBase.GetItemLists().Count))
        // {
        //     NumItem.Add(a[i],1);
        // }
        for(int i=0;i<itemDataBase.GetItemLists().Count;i+=1)
        {
            NumItem.Add(a[i],1);
        }
    }
}
