using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ItemDataBase",menuName="CreateItemDataBase")]
public class ItemDataBase : ScriptableObject 
{
	[SerializeField] private List<ItemBase> itemLists=new List<ItemBase>();

	public List<ItemBase> GetItemLists()
	{
		return itemLists;
	}
}
