using System.IO;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MoveObject
{
    private static readonly Dictionary<int, int> LevelUpExp;
    [SerializeField] private STATE _state;
    private bool AbnormalCondition;
    public List<GameObject> inventoryList;
    public int MaxHP { get; set; }
    public int Satiety { get; set; } //満腹度
    public int MaxSatiety { get; private set; } = 100;
    public STATE state { get { return _state; } set { _state = value; } }
   
    protected override void Start ()
    {
        base.Start();
        if (File.Exists($"{Application.persistentDataPath}{gameManager.FileName}")==false)
        {
            Level = 1;
            HP = 50;
            ATK = 10;
            MaxHP = HP;
        }
        Satiety = 100;
        direction = DIRECTION.DOWN;
        state = STATE.NONE;
	}

    private void Update()
    {
        //プレイヤーのターン
        if (gameManager.TurnPlayer == true)
        {
            //行動する(ポーズ時以外)
            if (gameManager.GamePause == false)
            {
                MovePlayer((int)gameObject.transform.position.x,
                           (int)gameObject.transform.position.y);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    AttackPlayer((int)gameObject.transform.position.x,
                                 (int)gameObject.transform.position.y);
                }
            }
            //状態異常の遷移
            switch (state)
            {
                case STATE.POISON:
                    HP -= 1;
                    break;
                case STATE.PARALYSIS:
                    gameManager.TurnPlayer = false;
                    break;
                default:
                    break;
            }
            if (state != STATE.NONE)
            {
                if (ReleaseDetermination() == true)
                {
                    state = STATE.NONE;
                }
            }
            //行動終了
            if (TurnEnd == true)
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
                TurnEnd = false;
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
        if (collision.gameObject.tag == "Exit")
        {
            Debug.Log("exit");
            gameManager.FloorNumber += 1;
            Destroy(GameObject.Find("Map"));
            Destroy(GameObject.Find("Enemy"));
            Destroy(GameObject.Find("Item"));
            mapGenerator.Awake();
            gameManager.Refrash();
            //mapGenerator.InitializeMap();
            //mapGenerator.RoomCreate();
            //mapGenerator.CreateDungeon();
            //gameManager.Start();
            gameManager.ListAdd();
            gameManager.RandomDeploy();
            gameManager.CameraOnCenter();
        }
    }
    #endregion

    #region プレイヤーの移動
    /// <summary>
    /// プレイヤーの移動判定
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
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
                else if(mapGenerator.MapStatusType[x,y+1]==(int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x-1, y] == (int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x+1, y] == (int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            default:
                return false;
        }
    }
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
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
                Vector3 Destination = new Vector3(gameObject.transform.position.x, 
                                                  gameObject.transform.position.y + 1, 
                                                  gameObject.transform.position.z);
                Vector3 vector3 = Vector3.zero;
               
                {
                    gameObject.transform.position = /*Vector3.SmoothDamp(gameObject.transform.position,
                                                                   Destination,
                                                                   ref vector3,
                                                                   1.0f);*/
                        Vector3.Lerp(gameObject.transform.position, Destination, 1);
                 }
                //gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                //                                            gameObject.transform.position.y + 1);
                gameManager.CameraOnCenter();
            }
            TurnEnd = true;
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

            TurnEnd = true;
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
            TurnEnd = true;
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
            TurnEnd = true;
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
                        for (int i = 0; i < gameManager.enemiesList.Count; i += 1)
                        {
                            if (gameManager.enemiesList[i].transform.position ==new Vector3(x, y + 1))
                            {
                                if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>().HP -= ATK;
                                }
                            }
                        }
                    }
                }
                TurnEnd = true;
                break;
            case DIRECTION.DOWN:
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {
                        for (int i = 0; i < gameManager.enemiesList.Count; i += 1)
                        {
                            if (gameManager.enemiesList[i].transform.position == new Vector3(x, y - 1))
                            {
                                if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>().HP -= ATK;
                                }
                            }
                        }
                    }
                }
                TurnEnd = true;
                break;
            case DIRECTION.LEFT:
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {
                        for (int i = 0; i < gameManager.enemiesList.Count; i += 1)
                        {
                            if (gameManager.enemiesList[i].transform.position == new Vector3(x - 1, y))
                            {
                                if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>().HP -= ATK;
                                }
                            }
                        }
                    }
                }
                TurnEnd = true;
                break;
            case DIRECTION.RIGHT:
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    JudgeAttack();
                    if (JudgeAttack() == true)
                    {
                        for (int i = 0; i < gameManager.enemiesList.Count; i += 1)
                        {
                            if (gameManager.enemiesList[i].transform.position == new Vector3(x + 1, y))
                            {
                                if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    gameManager.enemiesList[i].gameObject.GetComponent<EnemyKnight>().HP -= ATK;
                                }
                            }
                        }
                    }
                }
                TurnEnd = true;
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
        ATK = Mathf.RoundToInt(ATK * 1.2f);
        DEF = Mathf.RoundToInt(DEF * 1.2f);
    }
#endregion

}
