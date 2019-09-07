using System;
using UnityEngine;

public sealed class EnemyZombie : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        HP = 1;
        direction = DIRECTION.RIGHT;
    }
    protected override void Update()
    {
        base.Update();
    }
    
 
}
