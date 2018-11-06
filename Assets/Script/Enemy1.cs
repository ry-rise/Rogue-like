using System;
using UnityEngine;

public sealed class Enemy1 : MoveObject {
    //private GameManager gameManeger;
    private Transform playerPos;
    //private Animator enemyAnimator;
    
    protected override void Start()
    {
        HP = 10;
        //enemyAnimator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //gameManeger = GameObject.Find("GameManeger").GetComponent<GameManager>();
        base.Start();
    }
    private void Update()
    {
        if (gameManager.TurnEnemy == true)
        {
            MoveEnemy();
            if (funcEnd == true)
            gameManager.TurnPlayer = true;
            gameManager.TurnEnemy = false;
        }
    }
    public void MoveEnemy()
    {
        int mov_x = (int)(playerPos.position.x - gameObject.transform.position.x);
        int mov_y = (int)(playerPos.position.y - gameObject.transform.position.y);
        Vector2 mov = playerPos.position - gameObject.transform.position;
        mov.Normalize();
        if (Math.Abs(mov_x) > Math.Abs(mov_y))
        {
            if (mov_x < 0)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                            gameObject.transform.position.y);
            }
            else
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                            gameObject.transform.position.y);
            }
        }
        else
        {
            if (mov_y < 0)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                  gameObject.transform.position.y - 1);
            }
            else
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                         gameObject.transform.position.y + 1);
            }
        }
        funcEnd = true;
        #region a
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
        #endregion
    }

}
