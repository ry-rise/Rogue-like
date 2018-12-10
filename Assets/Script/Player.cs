//#define DEBUG_KEYDOWN
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MoveObject {
    //private int[] LevelUP_Exp = { 100,150 };
    public List<GameObject> inventoryList;
    public int Satiety { get; set; }//満腹度
    private DIRECTION direction;

    protected override void Start ()
    {
        Level = 1;
        HP = 100;
        Satiety = 100;
        direction = DIRECTION.DOWN;
        base.Start();
	}

    void Update()
    {
#if DEBUG_KEYDOWN
        if (Input.GetKeyDown(KeyCode.A)) { HP = 0; }
#endif
        //プレイヤーのターン
        if (gameManager.TurnPlayer == true)
        {
            //行動する
            MovePlayer((int)gameObject.transform.position.x,
                       (int)gameObject.transform.position.y);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AttackPlayer((int)gameObject.transform.position.x,
                             (int)gameObject.transform.position.y);
            }
            //行動終了
            if (funcEnd == true)
            {
                //空腹度が０
                if (Satiety == 0)
                {
                    HP -= 1;
                }
                //０以外
                else
                {
                    Satiety -= 1;
                }
                gameManager.TurnPlayer = false;
                //gameManager.TurnEnemy = true;
                funcEnd = false;
            }
        }
        //死ぬとシーンチェンジ
        if (HP <= 0)
        {
            sceneChanger.FromPlayToOver();
        }
    }

#region 判定       
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Exit")
        {
            Debug.Log("exit");
            //enabled = false;
            gameManager.FloorNumber += 1;
            mapGenerator.InitializeMap();
            mapGenerator.RoomCreate();
            mapGenerator.CreateDungeon();
            //sceneChanger.SceneChange();
        }
    }
    #endregion

    #region プレイヤーの移動
    private bool CheckMovePlayer(DIRECTION direction, int x, int y)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                return true;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                return true;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                return true;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                return true;
            default:
                return false;
        }
    }
    private void MovePlayer(int x,int y)
    {
        //上方向
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = DIRECTION.UP;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y + 1] = (int)MapGenerator.STATE.PLAYER;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            gameObject.transform.position.y + 1);
                gameManager.CameraOnCenter();
            }
            funcEnd = true;
        }
        //下方向
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = DIRECTION.DOWN;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y - 1] = (int)MapGenerator.STATE.PLAYER;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            gameObject.transform.position.y - 1);
                gameManager.CameraOnCenter();
            }

            funcEnd = true;
        }
        //左方向
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = DIRECTION.LEFT;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x-1, y] = (int)MapGenerator.STATE.PLAYER;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                            gameObject.transform.position.y);
                gameManager.CameraOnCenter();
            }
            funcEnd = true;
        }
        //右方向
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = DIRECTION.RIGHT;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y))
            {
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x + 1, y] = (int)MapGenerator.STATE.PLAYER;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                        gameObject.transform.position.y);
                gameManager.CameraOnCenter();
            }
            funcEnd = true;
        }
    }
    #endregion

    #region 攻撃
    private void AttackPlayer(int x,int y)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {

                    }
                }
                funcEnd = true;
                break;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {

                    }
                }
                funcEnd = true;
                break;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {

                    }
                }
                funcEnd = true;
                break;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {

                    }
                }
                funcEnd = true;
                break;
        }
    }
    private bool JudgeAttack()
    {
        int i = Random.Range(0, 2);
        if (i == 1) { return true; }
        else { return false; }
    }
#endregion

    #region レベルアップ時の挙動
    private void LevelUP()
    {
        Level += 1;
    }
#endregion

}
