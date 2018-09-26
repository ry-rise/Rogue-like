using System;
using UnityEngine;

public sealed class Enemy1 : MoveObject {
    private GameManeger gameManeger;
    //private Animator enemy_animator;
    private Transform player_pos;
    protected override void Start()
    {
        //enemy_animator = GetComponent<Animator>();
        player_pos = GameObject.FindGameObjectWithTag("Player").transform;
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        //base.Start();
    }
    private void Update()
    {
        if (gameManeger.turn_enemy == true)
        {
            MoveEnemy();
            if (func_end == true)
            gameManeger.turn_player = true;
            gameManeger.turn_enemy = false;
        }
    }
    public void MoveEnemy()
    {
        int mov_x = (int)(player_pos.position.x - gameObject.transform.position.x);
        int mov_y = (int)(player_pos.position.y - gameObject.transform.position.y);
        Vector2 mov = player_pos.position - gameObject.transform.position;
        mov.Normalize();
        if (Math.Abs(mov_x) > Math.Abs(mov_y))
        {
            if (mov_x<0) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                            gameObject.transform.position.y);
            }
            else {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                            gameObject.transform.position.y);
            }
        }
        else
        {
            if (mov_y<0) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                  gameObject.transform.position.y-1);
            }
            else {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                         gameObject.transform.position.y+1);
            }
        }
        func_end = true;
        //int xDir = 0;
        //int yDir = 0;
        //if (Mathf.Abs(player_pos.position.x - gameObject.transform.position.x) < float.Epsilon)
        //{
        //    //プレイヤーが上にいれば+1、下に入れば-1する
        //    yDir = player_pos.position.y > gameObject.transform.position.y ? 1 : -1;
        //    yDir += (int)gameObject.transform.position.y;
        //    func_end = true;
        //}
        //else
        //{
        //    //プレイヤーが右にいれば+1、左にいれば-1する
        //    xDir = player_pos.position.x > gameObject.transform.position.x ? 1 : -1;
        //    xDir += (int)gameObject.transform.position.x;
        //    func_end = true;
        //}
    }
}
