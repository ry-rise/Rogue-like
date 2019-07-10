using System.Collections.Generic;
using System.IO;
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
        //データが無ければ初期化
        if (File.Exists($"{Application.persistentDataPath}{gameManager.FileName}") == false)
        {
            Level = 1;
            HP = 50;
            ATK = 10;
            MaxHP = HP;
        }
        Satiety = 100;
        direction = DIRECTION.UP;
        state = STATE.NONE;
        SpriteDirection();
	}

    private void Update()
    {
        gameManager.CameraOnCenter();
        //敵の行動が終わったら
        if(gameManager.turnManager==GameManager.TurnManager.ENIMIES_END)
        {
            gameManager.turnManager = GameManager.TurnManager.PLAYER_TURN;
        }
        //プレイヤーのターン
        if (gameManager.turnManager==GameManager.TurnManager.PLAYER_TURN)
        {
            //行動する(ポーズ時以外)
            if (gameManager.GamePause == false)
            {
                MovePlayer((int)gameObject.transform.position.x,
                           (int)gameObject.transform.position.y);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameManager.turnManager = GameManager.TurnManager.PLAYER_ATTACK;
                    AttackPlayer((int)gameObject.transform.position.x,
                                 (int)gameObject.transform.position.y);
                    gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
                }
            }
            //状態異常の判定に移動
            if (gameManager.turnManager == GameManager.TurnManager.STATE_JUDGE)
            {
                //状態異常の遷移
                switch (state)
                {
                    case STATE.POISON:
                        HP -= 1;
                        if (ReleaseDetermination() == true) { state = STATE.NONE; }
                        break;
                    case STATE.PARALYSIS:
                        if (ReleaseDetermination() == true) { state = STATE.NONE; }
                        else { gameManager.turnManager = GameManager.TurnManager.PLAYER_END; }
                        break;
                    default:
                        break;
                }
                gameManager.turnManager = GameManager.TurnManager.SATIETY_CHECK;
            }

            //空腹度チェック
            if (gameManager.turnManager==GameManager.TurnManager.SATIETY_CHECK)
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
                //行動終了
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
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
            //Debug.Log("exit");
            gameManager.Exit();
            //gameManager.FloorNumber += 1;
            //Destroy(GameObject.Find("Map"));
            //Destroy(GameObject.Find("Enemy"));
            //Destroy(GameObject.Find("Item"));
            //mapGenerator.Awake();
            //gameManager.Refrash();
            ////mapGenerator.InitializeMap();
            ////mapGenerator.RoomCreate();
            ////mapGenerator.CreateDungeon();
            ////gameManager.Start();
            //gameManager.ListAdd();
            //gameManager.RandomDeploy();
            //gameManager.CameraOnCenter();
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
                bool isExit = false;
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                Vector2 prevPosition = gameObject.transform.position;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y + 1] = (int)MapGenerator.STATE.PLAYER;
                SpriteDirection();
                StartCoroutine(FrameWait(0.0001f, 0, 0.1f, MoveNum, DIRECTION.UP, prevPosition));
                if (isExit == true)
                {
                    gameManager.Exit();
                }
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //下方向
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = DIRECTION.DOWN;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                bool isExit = false;
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                Vector2 prevPosition = gameObject.transform.position;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x, y - 1] = (int)MapGenerator.STATE.PLAYER;
                SpriteDirection();
                //int num = 0;
                StartCoroutine(FrameWait(0.0001f, 0, -0.1f, MoveNum, DIRECTION.DOWN,prevPosition));
                if (isExit == true)
                {
                    gameManager.Exit();
                }
                //gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                //                                            gameObject.transform.position.y - 1);
            }

            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //左方向
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = DIRECTION.LEFT;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                bool isExit = false;
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                Vector2 prevPosition = gameObject.transform.position;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x-1, y] = (int)MapGenerator.STATE.PLAYER;
                SpriteDirection();
                //int num = 0;
                StartCoroutine(FrameWait(0.0001f, -0.1f, 0, MoveNum, DIRECTION.LEFT,prevPosition));
                if (isExit == true)
                {
                    gameManager.Exit();
                }
                //gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                //                                            gameObject.transform.position.y);
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //右方向
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = DIRECTION.RIGHT;
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y))
            {
                bool isExit = false;
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                Vector2 prevPosition = gameObject.transform.position;
                mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                mapGenerator.MapStatusType[x + 1, y] = (int)MapGenerator.STATE.PLAYER;
                SpriteDirection();
                //int num = 0;
                StartCoroutine(FrameWait(0.0001f, 0.1f, 0, MoveNum, DIRECTION.RIGHT,prevPosition));
                if(isExit==true)
                {
                    gameManager.Exit();
                }
                //gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                //                                        gameObject.transform.position.y);
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
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
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
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
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
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
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
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
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
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
        NextExp -= 1;
        ATK = Mathf.RoundToInt(ATK * 1.2f);
        DEF = Mathf.RoundToInt(DEF * 1.2f);
    }
#endregion

}
