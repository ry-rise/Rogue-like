using System;
using UnityEngine;

public sealed class EnemyZombie : EnemyBase
{
    protected override void Start()
    {
        HP = 1;
        direction = DIRECTION.DOWN;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    
 
}
