using UnityEngine;
//空腹度を回復するアイテム
public sealed class ItemRiceBall : ItemBase
{

    protected override void Awake ()
    {
        ID=0;
        Name = "おにぎり";
        base.Awake();
	}

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         //Debug.Log("itemRiceBall");
    //         PickUP();
    //     }
    // }
    protected override void Update()
    {
        base.Update();
    }
    protected override void PickUP()
    {
        base.PickUP();
    }
    

    public override void Use()
    {
        int i = Random.Range(0, recoveryAmount.Length);
        if (player.Satiety + recoveryAmount[i] > player.MaxSatiety)
        {
            player.Satiety = player.MaxSatiety;
        }
        else
        {
            player.Satiety += recoveryAmount[i];
        }
        Log.Instance.LogTextWrite($"HPを{recoveryAmount[i].ToString()}回復した");
        player.inventoryList.Remove(gameObject);
        Destroy(gameObject);
        GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
    }
}
