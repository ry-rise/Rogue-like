using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour {
    #region バッキングフィールド
    //[SerializeField] private int _hp;
    //[SerializeField] private int _level;
    //[SerializeField] private int _exp;
    //[SerializeField] private int _direction;
    #endregion
    private int State;//状態
    //private float MoveTime = 0.1f;
    //private float calMoveTime;//移動時間を計算
    protected int h = 0;
    protected int v = 0;
    protected bool func_end = false;
    #region プロパティ
    public int HP { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Direction { get; set; }
    #endregion
    enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    enum ACTION { TURN_STANDBY,ACT_START,ACT,ACT_END,MOVE_START,MOVING,MOVE_END,TURN_END }
    private BoxCollider2D boxCollider;
    //private Rigidbody2D rigidbody2;
    public LayerMask Hitlayer;
    
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //rigidbody2 = GetComponent<Rigidbody2D>();
        //calMoveTime = 1f / MoveTime;
        h = (int)Input.GetAxisRaw("Horizontal");
        v = (int)Input.GetAxisRaw("Vertical");
    }
    //protected bool MoveCheck(int x,int y,out RaycastHit2D hit)
    //{
    //    Vector2 startpos = transform.position;
    //    Vector2 endpos = startpos + new Vector2(x, y);
    //    boxCollider.enabled = false;
    //    hit = Physics2D.Linecast(startpos, endpos, Hitlayer);
    //    boxCollider.enabled = true;
    //    if (hit.transform == null)
    //    {
    //        StartCoroutine("Moving");
    //        return true;
    //    }
    //    return false;
    //}
    //protected IEnumerator Moving()
    //{
    //    if (h != 0 && v == 0)
    //    {
            
    //    }
    //    if (h == 0 && v != 0)
    //    {
            
    //    }
    //    yield return null;
    //}
    
}
