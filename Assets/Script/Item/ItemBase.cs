using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    private bool pickedUp = false;
    protected MapGenerator mapGenerator;
    protected UIManager iManager;
    protected Player player;
    protected int[] recoveryAmount = { 10, 20, 30, 40, 50 };
    public string Name { get; protected set; }
    public int ID { get; protected set; }

    protected virtual void Awake()
    {
        mapGenerator = GameObject.Find("Manager").GetComponent<MapGenerator>();
        iManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// アイテムを取った時、トラップに引っ掛かったときの処理
    /// </summary>
    protected virtual void PickUP()
    {
        if (pickedUp) return; //二重実行防止
        pickedUp = true;
        var col = GetComponent<BoxCollider2D>();
        if (col) col.enabled = false;
        var spr = GetComponent<SpriteRenderer>();
        if (spr) spr.enabled = false;
        if (mapGenerator != null)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            mapGenerator.MapStatusItem[x, y] = (int)MapGenerator.ITEM_STATE.NONE;
        }
        if (Log.Instance != null) Log.Instance.LogTextWrite($"{Name}を手に入れた");
    }

    public void Collect()
    {
        PickUP();
        gameObject.SetActive(false);
    }

    ///<summary>  
    ///アイテムを使うときの処理
    ///</summary>
    public virtual void Use() { }

    protected virtual void OnDisable() { }

}
