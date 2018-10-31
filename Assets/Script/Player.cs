//#define DEBUG_KEYDOWN
using UnityEngine;

public sealed class Player : MoveObject {
    //private GameManager gameManeger;
    //private const int ItemLimit = 99;
    public int Satiety { get; set; }//満腹度
    //private bool func_end = false;
    //public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }
    //private int[] LevelUP_Exp = { 100,150 };
    //private Animator player_animator;

    private void Awake()
    {
       
    }

    protected override void Start () {
        Level = 1;
        HP = 100;
        Satiety = 100;
        //player_animator = GetComponent<Animator>();
        //gameManeger = GameObject.Find("GameManeger").GetComponent<GameManager>();
        //sceneManeger = GameObject.Find("GameManeger").GetComponent<SceneManeger>();
        base.Start();
	}

    void Update()
    {
#if DEBUG_KEYDOWN
        if (Input.GetKeyDown(KeyCode.A)) { HP = 0; }
#endif
        if (gameManager.TurnPlayer == true)
        {
            MovePlayer();
            if (funcEnd == true)
            {
                if (Satiety == 0)
                {
                    HP -= 1;
                }
                else
                {
                    Satiety -= 1;
                }
                gameManager.TurnPlayer = false;
                gameManager.TurnEnemy = true;
                funcEnd = false;
            }
        }
        if (HP <= 0)
        {
            gameManager.SceneChange();
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
            gameManager.FloorNumber += 1;
            //sceneManeger.SceneChange();
            gameManager.SceneChange();
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
    private void MovePlayer()
    {
        //上方向
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                        gameObject.transform.position.y + 1);
            funcEnd = true;
        }
        //下方向
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                        gameObject.transform.position.y - 1);
            funcEnd = true;
        }
        //左方向
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 1,
                                                        gameObject.transform.position.y);
            funcEnd = true;
        }
        //右方向
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1,
                                                        gameObject.transform.position.y);
            funcEnd = true;
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
