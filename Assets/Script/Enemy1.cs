using UnityEngine;

public sealed class Enemy1 : EnemyBase {
    private Animator enemy_animator;
    private Transform player_pos;
    protected override void Start()
    {
        enemy_animator = GetComponent<Animator>();
        player_pos = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }
    protected override void CantMove<T>(T component)
    {
        
    }
}
