using UnityEngine;
using UnityEngine.UI;

public sealed class ButtonInventory : MonoBehaviour
{
    private Player player;
    [SerializeField] private int ListNumber;

	private void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();    
	}

    public void OnClick()
    {
        ItemPortion a = player.inventoryList[ListNumber].GetComponent<ItemPortion>();
        a.Use();
    }

    public void ItemUse<T>()
        where T : ItemBase
    {
        T t = player.inventoryList[ListNumber].GetComponent<T>();
        t.Use();
    }
}