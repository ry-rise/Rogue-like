using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour
{
    [NamedArray(new string[] { "UP", "DOWN", "LEFT", "RIGHT" })] [SerializeField]
    protected Sprite[] sprites = new Sprite[4];
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    //protected SceneChanger sceneChanger;
    protected SpriteRenderer spriteRenderer;
    protected DIRECTION direction;
    //移動変数
    protected int[] MoveNum = new int[4];
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
        //sceneChanger = GameObject.Find("Manager").GetComponent<SceneChanger>();
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
    /// <summary>
    /// 画像の向きを変える
    /// </summary>
    protected void SpriteDirection()
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
    protected IEnumerator SquaresMove(float moveX,float moveY,int num,DIRECTION direction,Vector2 prevPos)
    {
        if (num < 10)
        {
            gameObject.transform.position += new Vector3(moveX, moveY, 0);
            num += 1;
            StartCoroutine(FrameWait(0.0001f, moveX, moveY, num, direction,prevPos));
        }
        else
        {
            switch (direction)
            {
                case DIRECTION.UP:
                    gameObject.transform.position = new Vector2((int)gameObject.transform.position.x, (int)prevPos.y + 1);
                    break;
                case DIRECTION.DOWN:
                    gameObject.transform.position = new Vector2((int)gameObject.transform.position.x, (int)prevPos.y - 1);
                    break;
                case DIRECTION.LEFT:
                    gameObject.transform.position = new Vector2((int)prevPos.x - 1, (int)gameObject.transform.position.y);
                    break;
                case DIRECTION.RIGHT:
                    gameObject.transform.position = new Vector2((int)prevPos.x + 1, (int)gameObject.transform.position.y);
                    break;
            }
            gameManager.CameraOnCenter();
            num = 0;
            yield break;
        }

    }
    protected IEnumerator FrameWait(float waitTime,float moveX,float moveY,int num, DIRECTION direction, Vector2 prevPos)
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SquaresMove(moveX,moveY,num,direction,prevPos));
    }
}
