using UnityEngine;

public sealed class Item2 : ItemBase
{

    protected override void Awake ()
    {
        Name = "a";
        base.Awake();
	}
	
    protected override void PickUP()
    {
        
    }

    public override void Use()
    {

    }
}
