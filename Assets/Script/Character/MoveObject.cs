using UnityEngine;

public abstract class MoveObject : MonoBehaviour
{
    [NamedArray(new string[] { "UP", "DOWN", "LEFT", "RIGHT" })] [SerializeField]
    protected Sprite[] sprites = new Sprite[4];
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected SceneChanger sceneChanger;
    protected SpriteRenderer spriteRenderer;
    //protected bool TurnEnd { get; set; } = false;
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
}
