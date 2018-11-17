using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
    protected Player player;
    //アイテムを取った時、トラップに引っ掛かったときの処理
    protected abstract void PickUP(); 

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
