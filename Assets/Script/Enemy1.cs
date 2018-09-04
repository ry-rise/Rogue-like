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
    private void Update()
    {
        if (GameManeger.gameManeger.nowTurn ==GameManeger.TURN.TURN_ENEMY)
        {
            MoveEnemy();
            GameManeger.gameManeger.nowTurn = GameManeger.TURN.TURN_PLAYER;
        }
    }
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(player_pos.position.x - transform.position.x) < float.Epsilon)
        {
            //プレイヤーが上にいれば+1、下に入れば-1する
            yDir = player_pos.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            //プレイヤーが右にいれば+1、左にいれば-1する
            xDir = player_pos.position.x > transform.position.x ? 1 : -1;
        }
    }
}
