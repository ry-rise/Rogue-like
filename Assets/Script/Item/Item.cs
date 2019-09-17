public sealed class Item : ItemBase
{

	protected override void Awake ()
    {
        Name = "";
        base.Awake();
	}

    protected override void PickUP()
    {
        base.PickUP();
    }

    public override void Use()
    {
        
    }
}
