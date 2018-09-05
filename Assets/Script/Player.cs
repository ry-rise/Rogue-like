using UnityEngine;

public sealed class Player : MoveObject {
    private const int ItemLimit = 99;
    private const int Satiety = 100;//満腹度
    private int dec_Satiety;
    //public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }
    //public int Satiety { get { return _Satiety; }set { _Satiety = value; } }
    //private int[] LevelUP_Exp = { 100,150 };
    //enum Direction { UP, DOWN, LEFT, RIGHT }
    //private Direction dir;
    int px, py;
    private Animator player_animator;
    protected override void Start () {
        Level = 1;
        HP = 10;
        px = (int)GetComponent<Transform>().position.x;
        py = (int)GetComponent<Transform>().position.y;
        player_animator = GetComponent<Animator>();
        base.Start();
	}

    void Update()
    {
        //int h = 0;
        //int v = 0;
        //h = (int)Input.GetAxisRaw("Horizontal");
        //v = (int)Input.GetAxisRaw("Vertical");
        //if (h != 0)
        //{
        //    v = 0;
        //}
        //if (h != 0 || v != 0)
        //{

        //}
        if (GameManeger.gameManeger.nowTurn == GameManeger.TURN.TURN_PLAYER)
        {
            //上方向
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                py += 1;
                GameManeger.gameManeger.nowTurn = GameManeger.TURN.TURN_ENEMY;
            }
            //下方向
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                py -= 1;
                GameManeger.gameManeger.nowTurn = GameManeger.TURN.TURN_ENEMY;
            }
            //左方向
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                px -= 1;
                GameManeger.gameManeger.nowTurn = GameManeger.TURN.TURN_ENEMY;
            }
            //右方向
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                px += 1;
                GameManeger.gameManeger.nowTurn = GameManeger.TURN.TURN_ENEMY;
            }
        }
    }
    #region 当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            enabled = false;
        }
        if (collision.tag == "Item")
        {
            collision.gameObject.SetActive(false);
        }

    }
    #endregion

    //プレイヤーの移動
    private void Movement()
    {
        //上方向
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            py += 1;
        }
        //下方向
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            py -= 1;
        }
        //左方向
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            px -= 1;
        }
        //右方向
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            px += 1;
        }
    }

    #region 攻撃
    private void Attack()
    {

    }
    #endregion

    #region レベルアップ時の挙動
    private void LevelUP()
    {

    }
    #endregion

}
