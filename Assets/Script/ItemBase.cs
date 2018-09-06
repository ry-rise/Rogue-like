using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
    protected Player player;
    public abstract void PickUP(); //アイテムを取った時の処理
}
