﻿using UnityEngine;

public sealed class ItemRiceBall : ItemBase
{

    protected override void Awake ()
    {
        Name = "おにぎり";
        base.Awake();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("itemRiceBall");
            PickUP();
        }
    }

    protected override void PickUP()
    {
        base.PickUP();
        //for (int i = 0; i < iManager.LogText.Length; i += 1)
        //{
        //    if (iManager.LogText[i].text == "")
        //    {
        //        iManager.LogText[i].text = "おにぎりを手に入れた";
        //        break;
        //    }
        //    else { continue; }
        //}
        //if (iManager.LogText[iManager.LogText.Length - 1].text != "")
        //{
        //    iManager.LogText[iManager.LogText.Length - 1].text = "おにぎりを手に入れた";
        //}
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
        player.inventoryList.Remove(gameObject);
        Destroy(gameObject);
        gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
    }
}