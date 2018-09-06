using UnityEngine;

public sealed class Player : MoveObject {
    private GameManeger gameManeger;
    private const int ItemLimit = 99;
    private const int Satiety = 100;//満腹度
    private int dec_Satiety;
    //public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }
    //public int Satiety { get { return _Satiety; }set { _Satiety = value; } }
    //private int[] LevelUP_Exp = { 100,150 };
    private Transform pos;
    int px, py;
    //private Animator player_animator;
    private Rigidbody2D rigidbody2;

    protected override void Start () {
        Level = 1;
        HP = 10;
        pos = GetComponent<Transform>();
        px = (int)pos.position.x;
        py = (int)pos.position.y;
        rigidbody2 = GetComponent<Rigidbody2D>();
        //player_animator = GetComponent<Animator>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        //base.Start();
	}

    void Update()
    {
        if (gameManeger.turn_player == true)
        {
            //上方向
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                py += 1;
                gameManeger.turn_player = false;
                gameManeger.turn_enemy = true;
            }
            //下方向
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                py -= 1;
                gameManeger.turn_player = false;
                gameManeger.turn_enemy = true;
            }
            //左方向
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                px -= 1;
                gameManeger.turn_player = false;
                gameManeger.turn_enemy = true;
            }
            //右方向
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                px += 1;
                gameManeger.turn_player = false;
                gameManeger.turn_enemy = true;
            }
        }

        int a = (int)Input.GetAxisRaw("Horizontal");
        int b = (int)Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(a, b);
        rigidbody2.AddForce(move);
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
