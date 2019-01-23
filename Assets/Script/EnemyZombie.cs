using System;
using UnityEngine;

public sealed class EnemyZombie : MoveObject
{
    private Transform playerPos;
    private DIRECTION direction;
    protected override void Start()
    {
        HP = 10;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        direction = DIRECTION.DOWN;
        base.Start();
    }
    private bool CheckMoveEnemy(DIRECTION direction, int x, int y)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            default:
                return false;
        }
    }
    public void MoveEnemy(int x,int y)
    {
        
        int movX = (int)(playerPos.position.x - gameObject.transform.position.x);
        int movY = (int)(playerPos.position.y - gameObject.transform.position.y);
        Vector2 mov = playerPos.position - gameObject.transform.position;
        mov.Normalize();
        if (Math.Abs(movX) > Math.Abs(movY))
        {
            if (movX < 0)
            {
                direction = DIRECTION.LEFT;
                if (CheckMoveEnemy(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y)==true)
                {
                    mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusType[x - 1, y] = (int)MapGenerator.STATE.ENEMY;
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                                gameObject.transform.position.y);
                }
            }
            else
            {
                direction = DIRECTION.RIGHT;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x + 1, y] = (int)MapGenerator.STATE.ENEMY;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                            gameObject.transform.position.y);
            }
        }
        else
        {
            if (movY < 0)
            {
                direction = DIRECTION.DOWN;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y - 1] = (int)MapGenerator.STATE.ENEMY;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                  gameObject.transform.position.y - 1);
            }
            else
            {
                direction = DIRECTION.UP;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y + 1] = (int)MapGenerator.STATE.ENEMY;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                         gameObject.transform.position.y + 1);
            }
        }
        TurnEnd = true;
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
