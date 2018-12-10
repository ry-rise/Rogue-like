using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
    protected GameManager gameManager;
    protected Player player;
    //アイテムを取った時、トラップに引っ掛かったときの処理
    protected abstract void PickUP();
    //アイテムを使うときの処理
    protected virtual void Use() { }

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
