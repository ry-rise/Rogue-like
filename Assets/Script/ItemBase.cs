using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected GameManager gameManager;
    protected Player player;
    public string Name { get; protected set; }

    //アイテムを取った時、トラップに引っ掛かったときの処理
    protected abstract void PickUP();
    //アイテムを使うときの処理
    public virtual void Use() { }

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
