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

    private void Update()
    {
        //if (player.inventoryList[ListNumber] != null)
        //{
        //    transform.Find("Text").GetComponent<Text>().text
        //    = player.inventoryList[ListNumber].gameObject.GetComponent<ItemPortion>().Name;
        //}
    }

    public void OnClick()
    {
        ItemPortion a = player.inventoryList[ListNumber].GetComponent<ItemPortion>();
        a.Use();
    }
}