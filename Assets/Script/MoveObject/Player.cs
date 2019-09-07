using InputKey;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Player : MoveObject
{
    private static readonly Dictionary<int, int> LevelUpExp;
    [SerializeField] private STATE _state;
    private bool AbnormalCondition;
    public bool isExit { get; set; }
    public bool isMoving{get;set;}
    public List<GameObject> inventoryList;
    public int MaxHP { get; set; }
    public int Satiety { get; set; } //満腹度
    public int MaxSatiety { get; private set; } = 100;
    public STATE state { get { return _state; } set { _state = value; } }

    protected override void Start()
    {
        base.Start();
        //データが無ければ初期化
        if (File.Exists($"{Application.persistentDataPath}{DataManager.GameFileName}") == false)
        {
            Level = 1;
            HP = 100;
            ATK = 10;
            DEF = 5;
            MaxHP = HP;
            Satiety = 100;
            direction = DIRECTION.UP;
        }
        //移動可能かどうかを記憶
        foreach (var i in Enumerable.Range(0, Enum.GetValues(typeof(DIRECTION)).Length))
        {
            MoveNum[i] = 0;
        }
        state = STATE.NONE;
        SpriteDirection();
    }

    private void Update()
    {
        //ターン遷移
        switch (gameManager.turnManager)
        {
            //プレイヤーのターン
            case GameManager.TurnManager.PLAYER_START:
                //行動する(ポーズ時以外)
                if (gameManager.GamePause == false)
                {
                    if(isMoving==false)
                    {
                        MovePlayer((int)gameObject.transform.position.x,
                                   (int)gameObject.transform.position.y);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        DirectionMove();
                    }
                    if (InputManager.GridInputKeyDown(KeyCode.Return))
                    {
                        gameManager.turnManager = GameManager.TurnManager.PLAYER_ATTACK;
                        AttackPlayer((int)gameObject.transform.position.x,
                                     (int)gameObject.transform.position.y);
                        gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
                    }
                }
                break;

            case GameManager.TurnManager.STATE_JUDGE:
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
                break;

            case GameManager.TurnManager.SATIETY_CHECK:
                //空腹度が０
                if (Satiety == 0) { HP -= 1; }
                //０以外
                else { Satiety -= 1; }
                //行動終了
                gameManager.turnManager = GameManager.TurnManager.PLAYER_END;
                break;

            //敵の行動が終わったら
            case GameManager.TurnManager.ENIMIES_END:
                gameManager.turnManager = GameManager.TurnManager.PLAYER_START;
                break;
            default:
                break;
        }

        //死ぬとシーンチェンジ
        if (HP <= 0)
        {
            SceneChanger.ToOver();
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
                if (mapGenerator.MapStatusType[x, y + 1] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                else
                {
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusMoveObject[x, y + 1] = (int)MapGenerator.STATE.PLAYER;
                }
                isMoving=true;
                StartCoroutine(SquaresMove(0, 0.1f, MoveNum[(int)DIRECTION.UP], DIRECTION.UP, prevPosition));
                MoveNum[(int)DIRECTION.UP] = 0;
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
                if (mapGenerator.MapStatusType[x, y - 1] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                else
                {
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusMoveObject[x, y - 1] = (int)MapGenerator.STATE.PLAYER;
                }
                isMoving=true;
                StartCoroutine(SquaresMove(0, -0.1f, MoveNum[(int)DIRECTION.DOWN], DIRECTION.DOWN, prevPosition));
                MoveNum[(int)DIRECTION.DOWN] = 0;
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
                if (mapGenerator.MapStatusType[x - 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                else
                {
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusMoveObject[x - 1, y] = (int)MapGenerator.STATE.PLAYER;
                }
                isMoving=true;
                StartCoroutine(SquaresMove(-0.1f, 0, MoveNum[(int)DIRECTION.LEFT], DIRECTION.LEFT, prevPosition));
                MoveNum[(int)DIRECTION.LEFT] = 0;
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
                if (mapGenerator.MapStatusType[x + 1, y] == (int)MapGenerator.STATE.EXIT)
                {
                    isExit = true;
                }
                else
                {
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
                    mapGenerator.MapStatusMoveObject[x + 1, y] = (int)MapGenerator.STATE.PLAYER;
                }
                isMoving=true;
                StartCoroutine(SquaresMove(0.1f, 0, MoveNum[(int)DIRECTION.RIGHT], DIRECTION.RIGHT, prevPosition));
                MoveNum[(int)DIRECTION.RIGHT] = 0;
            }
            gameManager.turnManager = GameManager.TurnManager.STATE_JUDGE;
        }
    }

    private void DirectionMove()
    {
        if (InputManager.GridInputKeyDown(KeyCode.LeftShift, KeyCode.W))
        {
            direction = DIRECTION.UP;
            SpriteDirection();
            return;
        }
        else if (InputManager.GridInputKeyDown(KeyCode.LeftShift) && InputManager.GridInputKeyDown(KeyCode.S))
        {
            direction = DIRECTION.DOWN;
            SpriteDirection();
            return;
        }
        else if (InputManager.GridInputKeyDown(KeyCode.LeftShift) && InputManager.GridInputKeyDown(KeyCode.A))
        {
            direction = DIRECTION.LEFT;
            SpriteDirection();
            return;
        }
        else if (InputManager.GridInputKeyDown(KeyCode.LeftShift) && InputManager.GridInputKeyDown(KeyCode.D))
        {
            direction = DIRECTION.RIGHT;
            SpriteDirection();
            return;
        }
    }
    #endregion

    #region 攻撃
    private void AttackPlayerFunc<T>(Vector3 Vec3)
    where T : EnemyBase
    {
        if (mapGenerator.MapStatusType[(int)Vec3.x, (int)Vec3.y] == (int)MapGenerator.STATE.ENEMY)
        {
            if (JudgeAttack() == true)
            {
                foreach (var enemy in gameManager.enemiesList)
                {
                    if (enemy.transform.position == Vec3)
                    {
                        var enemyClass = enemy.GetComponent<T>();
                        if (enemyClass != null)
                        {
                            enemyClass.HP -= ATK;
                        }
                    }
                }
            }
        }
    }
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
                                if (enemy.GetComponent<EnemyZombie>() != null)
                                {
                                    enemy.GetComponent<EnemyZombie>().HP -= ATK;
                                }
                                else if (enemy.GetComponent<EnemyKnight>() != null)
                                {
                                    enemy.GetComponent<EnemyKnight>().HP -= ATK;
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
        ATK *= 2;
        DEF *= 2;
    }
    #endregion

}
