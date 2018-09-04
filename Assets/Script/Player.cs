using UnityEngine;

public sealed class Player : MoveObject {
    private const int ItemLimit = 99;
    private int Satiety;//満腹度
    //public int ItemLimit { get { return _itemLimit; } set { _itemLimit = value; } }
    //private int[] LevelUP_Exp = { 100,150 };
    enum Direction { UP, DOWN, LEFT, RIGHT }
    private Direction dir;
    int px, py;
    private Animator player_animator;
    protected override void Start () {
        Level = 1;
        HP = 10;
        px = (int)transform.position.x;
        py = (int)transform.position.y;
	}

    void Update()
    {
        int h = (int)Input.GetAxis("Horizontal");
        int v = (int)Input.GetAxis("Vertical");
        if (h != 0)
        {
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            h = 0;
        }
    }

    //プレイヤーの移動
    private bool Movement()
    {
        if (px == -1 || py == -1) { return true; }
        //上方向
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
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
        return true;
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

    protected override void OnCantMove<T>(T component)
    {
        
    }
}
