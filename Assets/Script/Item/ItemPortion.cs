using UnityEngine;
//HPを回復するアイテム
public sealed class ItemPortion : ItemBase
{    
    protected override void Awake()
    {
        ID=1;
        Name = "ポーション";
        base.Awake();
    }
   
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
        player.HP += recoveryAmount[i];
        //player.inventoryList.Remove(gameObject);
        Destroy(gameObject);
        GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
    }

}
