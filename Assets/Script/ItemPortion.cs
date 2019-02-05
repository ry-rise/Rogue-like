using UnityEngine;

public sealed class ItemPortion : ItemBase
{
    private int[] recoveryAmount = { 10, 20, 30, 40, 50 };
    
    protected override void Awake()
    {
        Name = "ポーション";
        base.Awake();
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("itemPortion");
            PickUP();
        }
    }

    protected override void PickUP()
    {
        player.inventoryList.Add(gameObject);
        gameManager.itemsList.Remove(gameObject);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Use()
    {
        int i = Random.Range(0, recoveryAmount.Length - 1);
        player.HP += recoveryAmount[i];
        player.inventoryList.Remove(gameObject);
        Destroy(gameObject);
        gameManager.TurnPlayer = false;
    }

}
