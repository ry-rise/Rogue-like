using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class GameData
{
    //インベントリ
    [SerializeField] private List<GameObject> _InventoryList;
    public List<GameObject> InventoryList { get { return _InventoryList; } set { _InventoryList = value; } }
    //何階層か
    [SerializeField] private int _FloorNumberData;
    public int FloorNumberData { get { return _FloorNumberData; } set { _FloorNumberData = value; } }
    //プレイヤーのHP
    [SerializeField] private int _HP;
    public int HP { get { return _HP; } set { _HP = value; } }
    //プレイヤーの最大HP
    [SerializeField] private int _MaxHP;
    public int MaxHP { get { return _MaxHP; } set { _MaxHP = value; } }
    //プレイヤーの攻撃力
    [SerializeField] private int _ATK;
    public int ATK { get { return _ATK; } set { _ATK = value; } }
    //プレイヤーのレベル
    [SerializeField] private int _Level;
    public int Level { get { return _Level; } set { _Level = value; } }
    //プレイヤーの経験値
    [SerializeField] private int _Exp;
    public int Exp { get { return _Exp; } set { _Exp = value; } }
    //プレイヤーの方向
    [SerializeField] private int _Direction;
    public int Direction { get { return _Direction; } set { _Direction = value; } }
    //プレイヤーの防御力
    [SerializeField] private int _DEF;
    public int DEF { get { return _DEF; } set { _DEF = value; } }
}


