﻿using System;
using UnityEngine;

public class EnemyBase : MoveObject 
{
    protected Transform playerPos;
    protected string Name;
    private GameObject player;
    private Player playerScript;
    protected bool check;
    private int flag = 0;
    private int flag_LEFT = 0x0001;
    private int flag_RIGHT = 0x0002;
    private int flag_UP = 0x0004;
    private int flag_DOWN = 0x0008;

    protected override void Start ()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        //playerSearch=gameObject.GetComponent<PlayerSearch>();
        playerPos = player.transform;
        Exp = 3;
	}
    protected virtual void Update()
    {
        if (HP <= 0)
        {
            DieEnemy();
        }
    }

    /// <summary>
    /// 敵が死んだときの処理
    /// </summary>
    protected void DieEnemy()
    {
        playerScript.Exp += Exp;
        GameManager.Instance.enemiesList.Remove(gameObject);
        mapGenerator.MapStatusType[(int)transform.position.x, (int)transform.position.y] = (int)MapGenerator.STATE.FLOOR;
        Destroy(gameObject);
    }
    /// <summary>
    /// 敵の移動判定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckMoveEnemy(DIRECTION direction, int x, int y)
    {
        switch (direction)
        {

            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    flag += flag_UP;
                    return false;
                }
                return true;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    flag+=flag_DOWN;
                    return false;
                }
                return true;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    flag += flag_LEFT;
                    return false;
                }
                return true;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    flag += flag_RIGHT;
                    return false;
                }
                return true;
            default:
                return false;
        }
    }
    /// <summary>
    /// 敵の移動処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveEnemy(int x, int y)
    {
        var a = Enum.GetValues(typeof(DIRECTION));
        var b = a.GetValue(new System.Random().Next(a.Length));
        switch ((DIRECTION)b)
        {
            case DIRECTION.UP:
                {
                    if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.UP;
                        mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusMoveObject[x, y + 1] = (int)MapGenerator.STATE.ENEMY;
                        SpriteDirection();
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                                    gameObject.transform.position.y + 1);
                        check = true;
                    }
                    else if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += flag_UP;
                    }
                    break;
                }
            case DIRECTION.DOWN:
                {
                    if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.DOWN;
                        mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusMoveObject[x, y - 1] = (int)MapGenerator.STATE.ENEMY;
                        SpriteDirection();
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                          gameObject.transform.position.y - 1);
                        check = true;
                    }
                    else if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += flag_DOWN;
                    }
                    break;
                }

            case DIRECTION.LEFT:
                {
                    if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.LEFT;
                        mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusMoveObject[x - 1, y] = (int)MapGenerator.STATE.ENEMY;
                        SpriteDirection();
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                                    gameObject.transform.position.y);
                        check = true;

                    }
                    else if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += flag_LEFT;
                    }
                    break;
                }
            case DIRECTION.RIGHT:
                {
                    if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.RIGHT;
                        mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusMoveObject[x + 1, y] = (int)MapGenerator.STATE.ENEMY;
                        SpriteDirection();
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                                    gameObject.transform.position.y);
                        check = true;
                    }
                    else if (CheckMoveEnemy((DIRECTION)b,(int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += flag_RIGHT;
                    }
                    break;
                }
            default:
                break;
        }

    }
    /// <summary>
    /// 敵の攻撃処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AttackEnemy(int x, int y)
    {
        if (CheckAttackEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
        {
            playerScript.HP -= (playerScript.DEF - ATK);
        }
        else { return; }
    }
    /// <summary>
    /// 敵の攻撃判定
    /// </summary>
    /// <param name="x">敵のX座標</param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckAttackEnemy(int x, int y)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    return true;
                }
                return false;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    return true;
                }
                return false;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    return true;
                }
                return false;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
  
}
