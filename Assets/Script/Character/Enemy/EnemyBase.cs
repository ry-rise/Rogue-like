using System;
using UnityEngine;

public class EnemyBase : MoveObject 
{
    protected Transform playerPos;
    protected string Name;
    private UIManager iManager;
    private GameObject player;
    private Player playerScript;
    private bool check;
    private int flag = 0;
    private int flag_LEFT = 0x0001;
    private int flag_RIGHT = 0x0002;
    private int flag_UP = 0x0004;
    private int flag_DOWN = 0x0008;

    protected override void Start ()
    {
        base.Start();
        iManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerPos = player.transform;
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
        gameManager.enemiesList.Remove(gameObject);
        mapGenerator.MapStatusType[(int)transform.position.x, (int)transform.position.y] = (int)MapGenerator.STATE.FLOOR;
        Destroy(gameObject);
    }
    /// <summary>
    /// 敵の移動判定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckMoveEnemy(int x, int y)
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
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
                    return false;
                }
                return true;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.PLAYER)
                {
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
        //int movX = (int)(playerPos.position.x - gameObject.transform.position.x);
        //int movY = (int)(playerPos.position.y - gameObject.transform.position.y);
        //Vector2 mov = playerPos.position - gameObject.transform.position;
        //mov.Normalize();
        //First:
        var a = Enum.GetValues(typeof(DIRECTION));
        var b = a.GetValue(new System.Random().Next(a.Length));
        /* (Math.Abs(movX) > Math.Abs(movY))*/
        //if (movX < 0)
        switch ((DIRECTION)b)
        {
            case DIRECTION.LEFT:
                {
                    if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.LEFT;
                        mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusType[x - 1, y] = (int)MapGenerator.STATE.ENEMY;
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                                    gameObject.transform.position.y);
                    }
                    else if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += 1000;
                        //goto First;
                    }
                    break;
                }
            case DIRECTION.RIGHT:
                {
                    if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.RIGHT;
                        mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusType[x + 1, y] = (int)MapGenerator.STATE.ENEMY;
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                                    gameObject.transform.position.y);
                    }
                    else if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += 0100;
                        //goto First;
                    }
                    break;
                }
            //}
            //else
            //{
            //if (movY < 0)
            case DIRECTION.DOWN:
                {
                    if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.DOWN;
                        mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusType[x, y - 1] = (int)MapGenerator.STATE.ENEMY;
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                          gameObject.transform.position.y - 1);
                    }
                    else if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += 0001;
                        //goto First;
                    }
                    break;
                }
            //else
            case DIRECTION.UP:
                {
                    if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
                    {
                        direction = DIRECTION.UP;
                        mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                        mapGenerator.MapStatusType[x, y + 1] = (int)MapGenerator.STATE.ENEMY;
                        gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                                    gameObject.transform.position.y + 1);
                    }
                    else if (CheckMoveEnemy((int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == false)
                    {
                        flag += 0001;
                        //goto First;
                    }
                    break;
                }
        }
        TurnEnd = true;

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
           
        }
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
