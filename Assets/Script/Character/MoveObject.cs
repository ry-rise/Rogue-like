using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour
{
    [NamedArray(new string[] { "UP", "DOWN", "LEFT", "RIGHT" })] [SerializeField]
    protected Sprite[] sprites = new Sprite[4];
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected SceneChanger sceneChanger;
    protected SpriteRenderer spriteRenderer;
    protected DIRECTION direction;
    //体力
    public int HP { get; set; }
    //レベル
    public int Level { get; set; }
    //経験値
    public int Exp { get; set; }
    //次のレベルまでの経験値
    public int NextExp { get; set; }
    //方向
    public int Direction { get; set; }
    //攻撃力
    public int ATK { get; set; }
    //防御力
    public int DEF { get; set; }
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    //状態異常
    public enum STATE { NONE, POISON, PARALYSIS }

    protected virtual void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        mapGenerator = GameObject.Find("Manager").GetComponent<MapGenerator>();
        sceneChanger = GameObject.Find("Manager").GetComponent<SceneChanger>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    /// <summary>
    /// 状態異常の解除判定
    /// </summary>
    protected bool ReleaseDetermination()
    {
        if (Random.Range(0, 5) == 0)
        {
            return true;
        }
        return false;
    }

    protected virtual void SpriteDirection()
    {
        switch(direction)
        {
            case DIRECTION.UP:
                spriteRenderer.sprite = sprites[(int)DIRECTION.UP];
                break;
            case DIRECTION.DOWN:
                spriteRenderer.sprite = sprites[(int)DIRECTION.DOWN];
                break;
            case DIRECTION.LEFT:
                spriteRenderer.sprite = sprites[(int)DIRECTION.LEFT];
                break;
            case DIRECTION.RIGHT:
                spriteRenderer.sprite = sprites[(int)DIRECTION.RIGHT];
                break;
        }
    }
    /// <summary>
    /// スムーズに移動する
    /// </summary>
    protected virtual void SquaresMove(GameObject moveObject,float posX,float posY,float moveX,float moveY,int num,float tx,float ty)
    {
        if(num!=10)
        {
            posX += moveX;
            posY += moveY;
            moveObject.transform.position += new Vector3(moveX, moveY,0);
            num += 1;
            StartCoroutine(enumerator(0.0001f, moveX, moveY, tx, ty));
        }
        else
        {
            Vector3 vector3 = moveObject.transform.position;
            vector3.x = tx;
            vector3.y = ty;
            moveObject.transform.position = vector3;
        }
    }
    protected IEnumerator enumerator(float waittime,float moveX,float moveY,float tx,float ty)
    {
        yield return new WaitForSeconds(waittime);
        SquaresMove(gameObject,
                    gameObject.transform.position.x,
                    gameObject.transform.position.y,
                    gameObject.transform.position.x + 1,
                    gameObject.transform.position.y,
                    1,
                    1.0f,
                    1.0f);
    }
    //void moving(float movex, float movey, int num, float tx, float ty)
    //{
    //    if (num != 10)
    //    {
    //        playerstates.px += movex;
    //        playerstates.py += movey;
    //        player.transform.position += new Vector3(movex, movey, 0);
    //        num++;
    //        StartCoroutin(DelayMove(0.0001f, movex, movey, tx, ty))
    //    }
    //    else
    //    {
    //        playerstates.px = tx;
    //        playerstates.py = ty;
    //        player.transform.position = new Vector3(tx, ty, 0)
    //   }
    //}
}
