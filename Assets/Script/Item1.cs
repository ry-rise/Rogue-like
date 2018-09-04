using UnityEngine;

public sealed class Item1 : ItemBase {
    private Player player;
    private int[] recovery_amount={10,20,30,40,50};
    
    public override void PickUP()
    {
        var i = Random.Range(0, 5);
        player.HP += recovery_amount[i];
    }
    
}
