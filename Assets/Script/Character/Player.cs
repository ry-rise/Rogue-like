using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using InputKey;

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

    protected override void Start()
    {
        base.Start();
        //データが無ければ初期化
        if (File.Exists($"{Application.persistentDataPath}{gameManager.FileName}") == false)
        {
            Level = 1;
            HP = 50;
            ATK = 10;
            DEF = 5;
            MaxHP = HP;
            Satiety = 100;
        }
        foreach (var i in Enumerable.Range(0, Enum.GetValues(typeof(DIRECTION)).Length))
        {
            MoveNum[i] = 0;
        }
        direction = DIRECTION.UP;
        state = STATE.NONE;
        SpriteDirection();
    }

    private void Update()
    {
        gameManager.CameraOnCenter();
        //敵の行動が終わったら
        if (gameManager.turnManager == GameManager.TurnManager.ENIMIES_END)
        {
            gameManager.turnManager = GameManager.TurnManager.PLAYER_START;
        }
        if (!(gameManager.turnManager == GameManager.TurnManager.PLAYER_START))
        {
            //StartCoroutine()
        }
        //プレイヤーのターン
        if (gameManager.turnManager == GameManager.TurnManager.PLAYER_START)
        {
            //行動する(ポーズ時以外)
            if (gameManager.GamePause == false)
            {
                MovePlayer((int)gameObject.transform.position.x,
                           (int)gameObject.transform.position.y);
                //gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameManager.turnManager = GameManager.TurnManager.PLAYER_ATTACK;
                    AttackPlayer((int)gameObject.transform.position.x,
                                 (int)gameObject.transform.position.y);
                    gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
                }
            }
        }
        //状態異常の判定に移動
        if (gameManager.turnManager == GameManager.TurnManager.STATE_JUDGE)
        {
            //状態異常の遷移
            switch (state)
            {
                case STATE.NONE:
                    break;
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
        if (gameManager.turnManager == GameManager.TurnManager.SATIETY_CHECK)
        {
            //空腹度が０
            if (Satiety == 0) { HP -= 1; }
            //０以外
            else { Satiety -= 1; }
            //行動終了
            gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
        }

        //死ぬとシーンチェンジ
        if (HP <= 0)
        {
            SceneChanger.FromPlayToOver();
        }
    }

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
                direction = DIRECTION.UP;
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            case DIRECTION.DOWN:
                direction = DIRECTION.DOWN;
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
                direction = DIRECTION.LEFT;
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.TRAP_POISON)
                {
                    state = STATE.POISON;
                    return true;
                }
                return true;
            case DIRECTION.RIGHT:
                direction = DIRECTION.RIGHT;
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.WALL ||
                    mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.ENEMY)
                {
                    return false;
                }
                else if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.TRAP_POISON)
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
    /// <param name="x">プレイヤーのx座標</param>
    /// <param name="y">プレイヤーのy座標</param>
    private void MovePlayer(int x, int y)
    {
        //上方向
        if (InputManager.GridInputKeyDown(KeyCode.W) || InputManager.GridInputKeyDown(KeyCode.UpArrow))
        {
            direction = DIRECTION.UP;
            SpriteDirection();
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                Vector2 prevPosition = gameObject.transform.position;
                if (mapGenerator.MapStatusType[x, y + 1] != (int)MapGenerator.STATE.EXIT)
                {
                    mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusType[x, y + 1] = (int)MapGenerator.STATE.PLAYER;
                }
                SpriteDirection();
                StartCoroutine(FrameWait(0.0001f, 0, 0.1f, MoveNum[(int)DIRECTION.UP], DIRECTION.UP, prevPosition));
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.EXIT)
                {
                    gameManager.Exit();
                }
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //下方向
        else if (InputManager.GridInputKeyDown(KeyCode.S) || InputManager.GridInputKeyDown(KeyCode.DownArrow))
        {
            direction = DIRECTION.DOWN;
            SpriteDirection();
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                Vector2 prevPosition = gameObject.transform.position;
                if (mapGenerator.MapStatusType[x, y - 1] != (int)MapGenerator.STATE.EXIT)
                {
                    mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusType[x, y - 1] = (int)MapGenerator.STATE.PLAYER;
                }
                SpriteDirection();
                StartCoroutine(FrameWait(0.0001f, 0, -0.1f, MoveNum[(int)DIRECTION.DOWN], DIRECTION.DOWN, prevPosition));
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.EXIT)
                {
                    gameManager.Exit();
                }

            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //左方向
        else if (InputManager.GridInputKeyDown(KeyCode.A) || InputManager.GridInputKeyDown(KeyCode.LeftArrow))
        {
            direction = DIRECTION.LEFT;
            SpriteDirection();
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y) == true)
            {
                Vector2 prevPosition = gameObject.transform.position;
                if (mapGenerator.MapStatusType[x - 1, y] != (int)MapGenerator.STATE.EXIT)
                {
                    mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusType[x - 1, y] = (int)MapGenerator.STATE.PLAYER;
                }
                SpriteDirection();
                StartCoroutine(FrameWait(0.0001f, -0.1f, 0, MoveNum[(int)DIRECTION.LEFT], DIRECTION.LEFT, prevPosition));
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    gameManager.Exit();
                }

            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
        //右方向
        else if (InputManager.GridInputKeyDown(KeyCode.D) || InputManager.GridInputKeyDown(KeyCode.RightArrow))
        {
            direction = DIRECTION.RIGHT;
            SpriteDirection();
            if (CheckMovePlayer(direction, (int)gameObject.transform.position.x, (int)gameObject.transform.position.y))
            {
                Vector2 prevPosition = gameObject.transform.position;
                if (mapGenerator.MapStatusType[x + 1, y] != (int)MapGenerator.STATE.EXIT)
                {
                    mapGenerator.MapStatusType[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusType[x + 1, y] = (int)MapGenerator.STATE.PLAYER;
                }
                SpriteDirection();
                StartCoroutine(FrameWait(0.0001f, 0.1f, 0, MoveNum[(int)DIRECTION.RIGHT], DIRECTION.RIGHT, prevPosition));
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    gameManager.Exit();
                }
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
    }

    private void DirectionMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W)) 
        { 
            direction=DIRECTION.UP;
            SpriteDirection();
            return;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKeyDown(KeyCode.S))
        {
            direction=DIRECTION.DOWN;
            SpriteDirection();
            return;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKeyDown(KeyCode.A))
        {
            direction=DIRECTION.LEFT;
            SpriteDirection();
            return;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKeyDown(KeyCode.D))
        {
            direction=DIRECTION.RIGHT;
            SpriteDirection();
            return;
        }
    }
    #endregion

    #region 攻撃
    private void AttackPlayer(int x, int y)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.ENEMY)
                {
                    if (JudgeAttack() == true)
                    {
                        foreach (var enemy in gameManager.enemiesList)
                        {
                            if (enemy.transform.position == new Vector3(x, y + 1))
                            {
                                if (enemy.gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (enemy.gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyKnight>().HP -= ATK;
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
                    if (JudgeAttack() == true)
                    {
                        foreach (var enemy in gameManager.enemiesList)
                        {
                            if (enemy.transform.position == new Vector3(x, y - 1))
                            {
                                if (enemy.gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (enemy.gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyKnight>().HP -= ATK;
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
                    if (JudgeAttack() == true)
                    {
                        foreach (var enemy in gameManager.enemiesList)
                        {
                            if (enemy.transform.position == new Vector3(x - 1, y))
                            {
                                if (enemy.gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (enemy.gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyKnight>().HP -= ATK;
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
                    if (JudgeAttack() == true)
                    {
                        foreach (var enemy in gameManager.enemiesList)
                        {
                            if (enemy.transform.position == new Vector3(x + 1, y))
                            {
                                if (enemy.gameObject.GetComponent<EnemyZombie>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (enemy.gameObject.GetComponent<EnemyKnight>() != null)
                                {
                                    enemy.gameObject.GetComponent<EnemyKnight>().HP -= ATK;
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
        ATK *= 2;//Mathf.RoundToInt(ATK * 1.2f);
        DEF *= 2; Mathf.RoundToInt(DEF * 1.2f);
    }
    #endregion

}
