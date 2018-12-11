using UnityEngine;

public sealed class Item1 : ItemBase {
    private int[] recoveryAmount = { 10, 20, 30, 40, 50 };

    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if (player)
        {
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("item1");
            PickUP();
        }
    }
    protected override void PickUP()
    {
        player.inventoryList.Add(gameObject);
        gameManager.items1List.Remove(gameObject);
        //gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //Destroy(gameObject);
    }

    protected override void Use()
    {
        int i = Random.Range(0, recoveryAmount.Length - 1);
        player.HP += recoveryAmount[i];
        player.inventoryList.Remove(gameObject);
    }

}
