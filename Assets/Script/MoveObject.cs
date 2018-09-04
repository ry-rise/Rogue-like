using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour {
    #region バッキングフィールド
    [SerializeField] private int _hp;
    [SerializeField] private int _level;
    //[SerializeField] private int _exp;
    //[SerializeField] private int _direction;
    #endregion
    private int State;//状態
    private int inverseMoveTime;
    #region プロパティ
    public int HP { get { return _hp; } set { _hp = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    //public int Exp { get { return _exp; } set { _exp = value; } }
    //public int _Direction { get { return _direction; } set { _direction = value; } }
    #endregion
    enum Direction { UP, DOWN, LEFT, RIGHT }
    enum Action { standby,act_start,act,act_end,move_start,_moving,move_end,turn_end }
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody2;
    public LayerMask Hitlayer;
    protected virtual void Updating() { }
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }
    protected bool MoveCheck(int x,int y,out RaycastHit2D hit)
    {
        Vector2 startpos = transform.position;
        Vector2 endpos = startpos + new Vector2(x, y);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(startpos, endpos, Hitlayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            StartCoroutine("Moving");
            return true;
        }
        return false;
    }
    protected IEnumerator Moving(Vector3 endpos)
    {
        //現在地から目的地を引き、2点間の距離を求める(Vector3型)
        //sqrMagnitudeはベクトルを2乗したあと2点間の距離に変換する(float型)
        float sqrRemainingDistance = (transform.position - endpos).sqrMagnitude;
        //2点間の距離が0になった時、ループを抜ける
        //Epsilon : ほとんど0に近い数値を表す
        while (sqrRemainingDistance > float.Epsilon)
        {
            //現在地と移動先の間を1秒間にinverseMoveTime分だけ移動する場合の、
            //1フレーム分の移動距離を算出する
            Vector3 newPosition = Vector3.MoveTowards(rigidbody2.position, endpos, inverseMoveTime * Time.deltaTime);
            //算出した移動距離分、移動する
            rigidbody2.MovePosition(newPosition);
            //現在地が目的地寄りになった結果、sqrRemainDistanceが小さくなる
            sqrRemainingDistance = (transform.position - endpos).sqrMagnitude;
            //1フレーム待ってから、while文の先頭へ戻る
            yield return null;
        }
    }

    //移動を試みるメソッド
    //virtual : 継承されるメソッドに付ける修飾子
    //<T>：ジェネリック機能　型を決めておかず、後から指定する
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        //ジェネリック用の型引数をComponent型で限定
        where T : Component
    {
        RaycastHit2D hit;
        //Moveメソッド実行 戻り値がtrueなら移動成功、falseなら移動失敗
        bool canMove = MoveCheck(xDir, yDir, out hit);
        //Moveメソッドで確認した障害物が何も無ければメソッド終了
        if (hit.transform == null)
        {
            return;
        }
        //障害物があった場合、障害物を型引数の型で取得
        //型が<T>で指定したものと違う場合、取得できない
        T hitComponent = hit.transform.GetComponent<T>();
        //障害物がある場合OnCantMoveを呼び出す
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    //abstract: メソッドの中身はこちらでは書かず、サブクラスにて書く
    //<T>：AttemptMoveと同じくジェネリック機能
    //障害物があり移動ができなかった場合に呼び出される
    protected abstract void OnCantMove<T>(T component) where T : Component;
}
