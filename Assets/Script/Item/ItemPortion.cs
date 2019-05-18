using UnityEngine;

public sealed class ItemPortion : ItemBase
{    
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
        base.PickUP();
    }

    public override void Use()
    {
        int i = Random.Range(0, recoveryAmount.Length);
        player.HP += recoveryAmount[i];
        player.inventoryList.Remove(gameObject);
        Destroy(gameObject);
        gameManager.TurnPlayer = false;
    }

}
