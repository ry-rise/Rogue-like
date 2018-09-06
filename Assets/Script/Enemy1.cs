using UnityEngine;

public sealed class Enemy1 : MoveObject {
    private GameManeger gameManeger;
    //private Animator enemy_animator;
    private Transform player_pos;
    private Transform enemy_pos;
    private int enemy_pos_x;
    private int enemy_pos_y;
    protected override void Start()
    {
        //enemy_animator = GetComponent<Animator>();
        player_pos = GameObject.FindGameObjectWithTag("Player").transform;
        enemy_pos = GetComponent<Transform>();
        enemy_pos_x = (int)enemy_pos.position.x;
        enemy_pos_y = (int)enemy_pos.position.y;
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        //base.Start();
    }
    private void Update()
    {
        if (gameManeger.turn_enemy == true)
        {
            MoveEnemy();
            gameManeger.turn_player = true;
            gameManeger.turn_enemy = false;
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
            enemy_pos_y += yDir;
        }
        else
        {
            //プレイヤーが右にいれば+1、左にいれば-1する
            xDir = player_pos.position.x > transform.position.x ? 1 : -1;
            enemy_pos_x += xDir;
        }
    }
}
