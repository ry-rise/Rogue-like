using UnityEngine;

public sealed class EnemyKnight : EnemyBase
{
    protected override void Start ()
    {
        base.Start();
        HP = 2;
        direction = DIRECTION.DOWN;
	}
	
	protected override void Update ()
    {
        base.Update();
	}
}
