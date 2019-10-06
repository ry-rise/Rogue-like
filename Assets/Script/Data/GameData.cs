using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class GameData
{
    //インベントリ
    [SerializeField] private List<GameObject> inventoryList;
    public List<GameObject> InventoryList { get { return inventoryList; } set { inventoryList = value; } }
    //何階層か
    [SerializeField] private int floorNumberData;
    public int FloorNumberData { get { return floorNumberData; } set { floorNumberData = value; } }
    //プレイヤーのHP
    [SerializeField] private int hP;
    public int HP { get { return hP; } set { hP = value; } }
    //プレイヤーの最大HP
    [SerializeField] private int maxHP;
    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    //プレイヤーの攻撃力
    [SerializeField] private int aTK;
    public int ATK { get { return aTK; } set { aTK = value; } }
    //プレイヤーのレベル
    [SerializeField] private int _Level;
    public int Level { get { return _Level; } set { _Level = value; } }
    //プレイヤーの経験値
    [SerializeField] private int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    //プレイヤーの方向
    [SerializeField] private int direction;
    public int Direction { get { return direction; } set { direction = value; } }
    //プレイヤーの防御力
    [SerializeField] private int dEF;
    public int DEF { get { return dEF; } set { dEF = value; } }
    //プレイヤーの空腹度
    [SerializeField] private int satiety;
    public int Satiety { get { return satiety; } set { satiety = value; } }
}


