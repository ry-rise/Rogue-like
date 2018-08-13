using UnityEngine;

public sealed class Player : MonoBehaviour {
    [SerializeField] private int hp;
    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int itemLimit;
    [SerializeField] private int _direction;
    public int HP { get { return hp; } set { hp = value; } }
    public int Level { get { return level; } set { level = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int ItemLimit { get { return itemLimit; } set { itemLimit = value; } }
    public int _Direction { get { return _direction; } set { _direction = value; } }
    enum Direction { UP, DOWN, LEFT, RIGHT }
    private Direction dir;
    void Start () {
        ItemLimit = 10;
	}
	
	void Update () {
		
	}

    //プレイヤーの移動
    private void Move()
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
