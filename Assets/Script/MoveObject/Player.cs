using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif
public sealed class Player : MoveObject
{
    private static readonly Dictionary<int, int> LevelUpExp;
    [ButtonDebug("aa", "a")] public int a;
    [SerializeField] private STATE _state;
    private bool AbnormalCondition;
    private PlayerStatus status;
    public bool isExit { get; set; }
    //public bool isMoving { get; set; }
    public List<ItemData> inventoryList;
    public int MaxHP { get; set; }
    public int Satiety { get; set; } //満腹度
    public int MaxSatiety { get; private set; } = 100;
    public STATE state { get { return _state; } set { _state = value; } }
    private PlayerInputRouter input;

    protected override void Start()
    {
        base.Start();
        status = GetComponent<PlayerStatus>();
        input = GetComponent<PlayerInputRouter>();
        if (status == null)
        {
            Debug.LogError("[Player] PlayerStatus が付いていません");
            enabled = false;
            return;
        }
        if (input == null)
        {
            Debug.LogError("[Player] PlayerInputRouter が付いていません");
            enabled = false;
            return;
        }
        //データが無ければ初期化
        if (File.Exists($"{Application.persistentDataPath}{DataManager.GameFileName}") == false)
        {
            status.InitDefaultForNewGame();
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
        switch (GameManager.Instance.turnManager)
        {
            //プレイヤーのターン
            case GameManager.TurnManager.PlayerStart:
                //行動する(ポーズ時以外)
                if (GameManager.Instance.GamePause) return;

                //Shift押しながら方向入力＝向きだけ変更（ターン消費しない）
                bool shiftHeld = UnityEngine.InputSystem.Keyboard.current?.leftShiftKey.isPressed ?? false;
                if (shiftHeld && input.MoveDir != Vector2Int.zero)
                {
                    direction = DirFromMove(input.MoveDir);
                    SpriteDirection();
                    input.Consume();//入力フラグをクリア
                    break;
                }

                //攻撃(submit) = ターン消費
                if (input.SubmitPressed)
                {
                    GameManager.Instance.turnManager = GameManager.TurnManager.PlayerAttack;
                    AttackPlayer((int)transform.position.x, (int)transform.position.y);
                    GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
                    input.Consume();
                    break;
                }

                //移動 = ターン消費
                if (!isMoving && input.MoveDir != Vector2Int.zero)
                {
                    MovePlayerByDir((int)transform.position.x, (int)transform.position.y, input.MoveDir);
                    GameManager.Instance.turnManager = GameManager.TurnManager.StateJudge;
                    input.Consume();
                }
                break;

            case GameManager.TurnManager.StateJudge:
                //状態異常の遷移
                status.ApplyStateTurnEffects();
                GameManager.Instance.turnManager = GameManager.TurnManager.SatietyCheck;
                break;

            case GameManager.TurnManager.SatietyCheck:
                //空腹度チェック
                status.ApplySatietyTurnEffects();
                //行動終了
                GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
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

    #region 攻撃
    private void AttackPlayer(int x, int y)
    {
        Vector3 target = TargetVectorFromDirection(direction, x, y);
        if (mapGenerator.MapStatusType[(int)target.x, (int)target.y] == (int)MapGenerator.STATE.ENEMY)
        {
            if (JudgeAttack())
            {
                foreach (var enemy in GameManager.Instance.enemiesList)
                {
                    if (enemy.transform.position == target)
                    {
                        var enemyBase = enemy.GetComponent<EnemyBase>();
                        if (enemyBase != null)
                        {
                            int damage = Mathf.Max(1, ATK - enemyBase.DEF);
                            enemyBase.HP -= damage;
                            Log.Instance?.LogTextWrite($"敵に{damage}ダメージ与えた");
                        }
                        break;
                    }
                }
            }
            else
            {
                Log.Instance?.LogTextWrite("攻撃はミスした！");
            }
        }
        GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
    }

    private Vector3 TargetVectorFromDirection(DIRECTION dir, int x, int y)
    {
        switch (dir)
        {
            case DIRECTION.UP: return new Vector3(x, y + 1);
            case DIRECTION.DOWN: return new Vector3(x, y - 1);
            case DIRECTION.LEFT: return new Vector3(x - 1, y);
            case DIRECTION.RIGHT: return new Vector3(x + 1, y);
            default: return new Vector3(x, y);
        }
    }
    #endregion

    public bool TryReleaseState()
    {
        return ReleaseDetermination();
    }

    private DIRECTION DirFromMove(Vector2Int move)
    {
        if (move.y > 0) return DIRECTION.UP;
        if (move.y < 0) return DIRECTION.DOWN;
        if (move.x < 0) return DIRECTION.LEFT;
        return DIRECTION.RIGHT;
    }

    private void MovePlayerByDir(int x, int y, Vector2Int move)
    {
        direction = DirFromMove(move);
        SpriteDirection();

        if (!CheckMovePlayer(direction, x, y)) return;

        Vector2 prevPosition = transform.position;

        int nx = x + move.x;
        int ny = y + move.y;

        if (mapGenerator.MapStatusType[nx, ny] == (int)MapGenerator.STATE.EXIT)
        {
            isExit = true;
        }
        else
        {
            mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;
            mapGenerator.MapStatusMoveObject[nx, ny] = (int)MapGenerator.STATE.PLAYER;
        }
        GameManager.Instance.TryPickupItemAt(nx, ny);
        isMoving = true;

        // 既存のSquaresMoveに合わせて delta を作る
        float dx = move.x * 0.1f;
        float dy = move.y * 0.1f;

        StartCoroutine(SquaresMove(dx, dy, MoveNum[(int)direction], direction, prevPosition));
        MoveNum[(int)direction] = 0;
    }

    public void RefreshDirectionSprite()
    {
        SpriteDirection();
    }
}

