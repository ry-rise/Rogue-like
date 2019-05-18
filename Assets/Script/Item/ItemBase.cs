using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected UIManager iManager;
    protected Player player;
    protected int[] recoveryAmount = { 10, 20, 30, 40, 50 };

    public string Name { get; protected set; }
    /// <summary>
    /// アイテムを取った時、トラップに引っ掛かったときの処理
    /// </summary>
    protected virtual void PickUP()
    {
        player.inventoryList.Add(gameObject);
        gameManager.itemsList.Remove(gameObject);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mapGenerator.MapStatusType[(int)transform.position.x, (int)transform.position.y] = (int)MapGenerator.STATE.FLOOR;
        for (int i = 0; i < iManager.LogText.Length; i += 1)
        {
            if (iManager.LogText[i].text == "")
            {
                iManager.LogText[i].text = $"{Name}を手に入れた";
                break;
            }
            else { continue; }
        }
        if (iManager.LogText[iManager.LogText.Length - 1].text != "")
        {
            iManager.LogText[iManager.LogText.Length - 1].text = $"{Name}を手に入れた";
        }
    }
    ///<summary>  
    ///アイテムを使うときの処理
    ///</summary>
    public virtual void Use() { }

    protected virtual void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        mapGenerator = GameObject.Find("Manager").GetComponent<MapGenerator>();
        iManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
