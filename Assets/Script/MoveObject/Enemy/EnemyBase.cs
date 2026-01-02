using System;
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

    protected override void Start()
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
                    flag += flag_DOWN;
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
    [SerializeField] private int chaseRange = 8; // この距離以内なら追跡（好みで調整）

    private int TileX(Transform t) => Mathf.RoundToInt(t.position.x);
    private int TileY(Transform t) => Mathf.RoundToInt(t.position.y);

    private bool TryAttackIfAdjacent()
    {
        int ex = TileX(transform);
        int ey = TileY(transform);
        int px = TileX(playerPos);
        int py = TileY(playerPos);

        int manhattan = Mathf.Abs(ex - px) + Mathf.Abs(ey - py);
        if (manhattan != 1) return false;

        // ダメ計算（今の式は回復しうるので修正）
        int damage = Mathf.Max(1, ATK - playerScript.DEF); // 0ダメOKなら Max(0, ...)
        playerScript.HP -= damage;

        Log.Instance?.LogTextWrite($"プレイヤーは{damage}ダメージ受けた");
        return true;
    }

    private bool CanMove(int x, int y, DIRECTION dir)
    {
        return CheckMoveEnemy(dir, x, y);
    }

    private bool TryMove(int x, int y, DIRECTION dir)
    {
        if (!CanMove(x, y, dir)) return false;

        direction = dir;
        mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.FLOOR;

        switch (dir)
        {
            case DIRECTION.UP:
                mapGenerator.MapStatusMoveObject[x, y + 1] = (int)MapGenerator.STATE.ENEMY;
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                break;

            case DIRECTION.DOWN:
                mapGenerator.MapStatusMoveObject[x, y - 1] = (int)MapGenerator.STATE.ENEMY;
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                break;

            case DIRECTION.LEFT:
                mapGenerator.MapStatusMoveObject[x - 1, y] = (int)MapGenerator.STATE.ENEMY;
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                break;

            case DIRECTION.RIGHT:
                mapGenerator.MapStatusMoveObject[x + 1, y] = (int)MapGenerator.STATE.ENEMY;
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                break;
        }

        SpriteDirection();
        check = true;
        return true;
    }

    private DIRECTION RandomDir()
    {
        var a = (DIRECTION[])System.Enum.GetValues(typeof(DIRECTION));
        return a[new System.Random().Next(a.Length)];
    }


    /// <summary>
    /// 敵の移動処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveEnemy(int x, int y)
    {
        // 0) 念のため null ガード（起動順事故対策）
        if (playerPos == null || playerScript == null || mapGenerator == null) return;

        // 1) 隣接してたら攻撃して終了（最優先）
        if (TryAttackIfAdjacent())
            return;

        // 2) 距離をタイル座標で計算
        int ex = TileX(transform);
        int ey = TileY(transform);
        int px = TileX(playerPos);
        int py = TileY(playerPos);

        int dist = Mathf.Abs(ex - px) + Mathf.Abs(ey - py);

        // 3) 追跡するか、徘徊するか
        if (dist <= chaseRange)
        {
            // 追跡：プレイヤーに近づく方向を優先
            int dx = px - ex;
            int dy = py - ey;

            // まず主軸（距離が大きい方）を試す
            DIRECTION primary;
            DIRECTION secondary;

            if (Mathf.Abs(dx) >= Mathf.Abs(dy))
            {
                primary = dx >= 0 ? DIRECTION.RIGHT : DIRECTION.LEFT;
                secondary = dy >= 0 ? DIRECTION.UP : DIRECTION.DOWN;
            }
            else
            {
                primary = dy >= 0 ? DIRECTION.UP : DIRECTION.DOWN;
                secondary = dx >= 0 ? DIRECTION.RIGHT : DIRECTION.LEFT;
            }

            // 主軸→副軸→（両方ダメなら）ランダムで数回トライ
            if (TryMove(x, y, primary)) return;
            if (TryMove(x, y, secondary)) return;

            // 詰まってるとき：ランダムに最大4回試す
            for (int i = 0; i < 4; i++)
            {
                var dir = RandomDir();
                if (TryMove(x, y, dir)) return;
            }

            // どこにも動けないなら何もしない
            return;
        }
        else
        {
            // 徘徊：完全ランダム移動（最大4回試す）
            for (int i = 0; i < 4; i++)
            {
                var dir = RandomDir();
                if (TryMove(x, y, dir)) return;
            }
            return;
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
