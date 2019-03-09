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
        for (int i = 0; i < iManager.LogText.Length; i += 1)
        {
            if (iManager.LogText[i].text == "")
            {
                iManager.LogText[i].text = "ポーションを手に入れた";
                break;
            }
            else { continue; }
        }
        if(iManager.LogText[iManager.LogText.Length-1].text!="")
        {
            iManager.LogText[iManager.LogText.Length - 1].text = "ポーションを手に入れた";
        }
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
