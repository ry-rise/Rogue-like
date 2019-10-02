using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    //protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected UIManager iManager;
    protected Player player;
    protected int[] recoveryAmount = { 10, 20, 30, 40, 50 };
    public string Name { get; protected set; }
    public int ID{get; protected set;}

    protected virtual void Awake()
    {
        //gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        mapGenerator = GameObject.Find("Manager").GetComponent<MapGenerator>();
        iManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// アイテムを取った時、トラップに引っ掛かったときの処理
    /// </summary>
    protected virtual void PickUP()
    {
        player.inventoryList.Add(gameObject);
        //GameManager.Instance.itemsList.Remove(gameObject);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mapGenerator.MapStatusType[(int)transform.position.x, (int)transform.position.y] = (int)MapGenerator.STATE.FLOOR;
        UIManager.LogTextWrite($"{Name}を手に入れた");
    }
    ///<summary>  
    ///アイテムを使うときの処理
    ///</summary>
    public virtual void Use() { }

    ///<summary>
    ///Playerの位置と一致すると取る
    ///</summary>    
    protected virtual void Update()
    {
        if(gameObject.transform.position.x==player.gameObject.transform.position.x&&
           gameObject.transform.position.y==player.gameObject.transform.position.y)
           {
               gameObject.SetActive(false);
           }
    }
    protected virtual void OnDisable()
    {
        PickUP();
    }
}
