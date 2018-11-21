using UnityEngine;

public sealed class Item1 : ItemBase {
    private int[] recoveryAmount = { 10, 20, 30, 40, 50 };

    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUP();
            Destroy(gameObject);
        }
    }
    protected override void PickUP()
    {
        Debug.Log($"PlusHP={recoveryAmount[0]}");
        var i = Random.Range(0, recoveryAmount.Length);
        player.HP += recoveryAmount[i];
    }
    
}
