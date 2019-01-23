using UnityEngine;

public sealed class TrapPoison : ItemBase
{

	protected override void Start ()
    {
        base.Start();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUP();
    }

    protected override void PickUP()
    {

    }
}
