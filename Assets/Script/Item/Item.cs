public sealed class Item : ItemBase
{

	protected override void Awake ()
    {
        ID=2;
        Name = "";
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
        
    }
}
