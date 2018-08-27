using UnityEngine;

public sealed class Player : MoverObject {
    [SerializeField] private int _itemLimit;
    public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }

    enum Direction { UP, DOWN, LEFT, RIGHT }
    private Direction dir;

    void Start () {
        ItemLimit = 10;
	}
	
	void Update () {
		
	}

    //プレイヤーの移動
    public override void Move()
    {
        //上方向
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            
        }
        //下方向
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {

        }
        //左方向
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

        }
        //右方向
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {

        }
    }

    //攻撃
    private void Attack()
    {

    }

    //レベルアップ時の挙動
    private void LevelUP()
    {

    }
}
