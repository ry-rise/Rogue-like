using System;
using UnityEngine;

public sealed class Enemy1 : MoveObject {
    private Transform playerPos;
    
    protected override void Start()
    {
        HP = 10;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }
    
    public void MoveEnemy()
    {
        int movX = (int)(playerPos.position.x - gameObject.transform.position.x);
        int movY = (int)(playerPos.position.y - gameObject.transform.position.y);
        Vector2 mov = playerPos.position - gameObject.transform.position;
        mov.Normalize();
        if (Math.Abs(movX) > Math.Abs(movY))
        {
            if (movX < 0)
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
            if (movY < 0)
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
