using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]private int hp = 100;//ヒットポイント
    //private int Level = 1;//レベル
    //private int Exp = 0;//経験値
    
    public int Hp { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int ItemLimit { get; set; }
    public int Direction { get; set; }
    enum{ UP, DOWN, LEFT, RIGHT }

    void Start () {
        ItemLimit = 10;
	}
	
	void Update () {
		
	}

    //プレイヤーの移動
    private void Move()
    {
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {

        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

        }
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
