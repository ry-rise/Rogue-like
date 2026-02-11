using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour
{
    [NamedArray(new string[] { "UP", "DOWN", "LEFT", "RIGHT" })]
    [SerializeField]
    protected Sprite[] sprites = new Sprite[4];
    protected MapGenerator mapGenerator;
    protected SpriteRenderer spriteRenderer;
    protected DIRECTION direction;
    //移動変数
    protected int[] MoveNum = new int[4];
    //移動判定
    protected bool isMoving { get; set; }
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
        mapGenerator = GameObject.Find("Manager").GetComponent<MapGenerator>();
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
        switch (direction)
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
    protected IEnumerator SquaresMove(float moveX, float moveY, int num, DIRECTION direction, Vector2 prevPos)
    {
        Vector3 from = transform.position;
        Vector3 to = from;

        switch (direction)
        {
            case DIRECTION.UP: to = new Vector3((int)prevPos.x, (int)prevPos.y + 1, from.z); break;
            case DIRECTION.DOWN: to = new Vector3((int)prevPos.x, (int)prevPos.y - 1, from.z); break;
            case DIRECTION.LEFT: to = new Vector3((int)prevPos.x - 1, (int)prevPos.y, from.z); break;
            case DIRECTION.RIGHT: to = new Vector3((int)prevPos.x + 1, (int)prevPos.y, from.z); break;
        }
        // 移動にかける時間（秒）：好みで調整
        const float duration = 0.12f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // EaseInOut（滑らか開始→停止）
            float eased = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(from, to, eased);

            // カメラ追従（存在チェックは安全のため）
            if (GameManager.Instance != null && GameManager.Instance.mainCamPos != null)
                GameManager.Instance.mainCamPos.transform.position = transform.position;

            yield return null; // フレーム同期が一番滑らか
        }
        transform.position = to;

        if (GameManager.Instance != null && GameManager.Instance.mainCamPos != null)
            GameManager.Instance.mainCamPos.transform.position = transform.position;

        isMoving = false;
    }
}
