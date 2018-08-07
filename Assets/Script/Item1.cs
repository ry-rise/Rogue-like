using UnityEngine;

public sealed class Item1 : ItemBase {
    private Player player;
    void Start()
    {
        player = new Player();
    }
    void Update()
    {

    }
    public override void PickUP()
    {
        player.HP += 10;
    }
    public override void A()
    {
        
    }
    
}
