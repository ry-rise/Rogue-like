//#define DEBUG_KEYDOWN
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MoveObject {
    //private int[] LevelUP_Exp = { 100,150 };
    public List<GameObject> Inventory;
    public int Satiety { get; set; }//満腹度
    private DIRECTION direction;

    protected override void Start ()
    {
        Level = 1;
        HP = 100;
        Satiety = 100;
        base.Start();
	}

    void Update()
    {
#if DEBUG_KEYDOWN
        if (Input.GetKeyDown(KeyCode.A)) { HP = 0; }
#endif
        //プレイヤーのターン
        if (gameManager.TurnPlayer == true)
        {
            //行動する
            MovePlayer();
            //行動終了
            if (funcEnd == true)
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
                //gameManager.TurnEnemy = true;
                funcEnd = false;
            }
        }
        //死ぬとシーンチェンジ
        if (HP <= 0)
        {
            sceneChanger.SceneChange();
        }
    }

#region 当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            enabled = false;
            gameManager.FloorNumber += 1;
            sceneChanger.SceneChange();
        }
    }
#endregion

#region プレイヤーの移動
    private bool MoveCheck(int x,int y)
    {
        if (mapGenerator.mapStatus[x, y] == -1)
        {
            return false;
        }
        return true;
    }
    private void MovePlayer()
    {
        //上方向
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //mapGenerator.mapStatus[]
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
        if (Input.GetKeyDown(KeyCode.Return))
        {

        }
    }
#endregion

#region レベルアップ時の挙動
    private void LevelUP()
    {
        Level += 1;
    }
#endregion

}
