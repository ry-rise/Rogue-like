//#define DEBUG_1
using UnityEngine;

public sealed class Player : MoveObject {
    private GameManeger gameManeger;
    private SceneManeger sceneManeger;
    private MapGenerator mapGenerator;
    //private const int ItemLimit = 99;
    public int Satiety { get; set; }//満腹度
    //private bool func_end = false;
    //public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }
    //private int[] LevelUP_Exp = { 100,150 };
    //private Animator player_animator;
    //private Rigidbody2D rigidbody2;

    private void Awake()
    {
       
    }

    protected override void Start () {
        Level = 1;
        HP = 100;
        Satiety = 100;
        //rigidbody2 = GetComponent<Rigidbody2D>();
        //player_animator = GetComponent<Animator>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        sceneManeger = GameObject.Find("GameManeger").GetComponent<SceneManeger>();
        //base.Start();
	}

    void Update()
    {
#if DEBUG_1
        if (Input.GetKeyDown(KeyCode.A)) { HP = 0; }
#endif
        if (gameManeger.turn_player == true)
        {
            Movement();
            if (func_end == true)
            {
                if (Satiety == 0)
                {
                    HP -= 1;
                }
                else
                {
                    Satiety -= 1;
                }
                gameManeger.turn_player = false;
                gameManeger.turn_enemy = true;
                func_end = false;
            }
        }
        if (HP <= 0)
        {
            sceneManeger.SceneChange();
        }
    }

#region 当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Wall")
        {
            var a = gameObject.transform.position.x;
            a -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            enabled = false;
            //sceneManeger.SceneChange();
            gameManeger.SceneChange();
        }
    }
#endregion

#region プレイヤーの移動
    private bool MoveCheck(int x,int y)
    {
        //if (mapGenerator.map_status[x, y] == -1)
        //{
        //    return false;
        //}
        return true;
    }
    private void Movement()
    {
        //上方向
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                        gameObject.transform.position.y + 1);
            func_end = true;
        }
        //下方向
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                        gameObject.transform.position.y - 1);
            func_end = true;
        }
        //左方向
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                        gameObject.transform.position.y);
            func_end = true;
        }
        //右方向
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                        gameObject.transform.position.y);
            func_end = true;
        }
    }
#endregion

#region 攻撃
    private void Attack()
    {

    }
#endregion

#region レベルアップ時の挙動
    private void LevelUP()
    {
        Level += 1;
    }
#endregion

}
