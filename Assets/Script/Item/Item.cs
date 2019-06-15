public class Item : ItemBase
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
}
