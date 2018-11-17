using UnityEngine;

public class Trap1 : ItemBase {

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
