using UnityEngine;

public sealed class EnemyKnight : EnemyBase
{
    protected override void Start ()
    {
        HP = 2;
        direction = DIRECTION.DOWN;
        base.Start();
	}
	
	protected override void Update ()
    {
        base.Update();
	}
}
