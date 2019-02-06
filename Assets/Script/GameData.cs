using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public sealed class GameData
{
    [SerializeField] private List<GameObject> _InventoryList;
    public List<GameObject> InventoryList { get { return _InventoryList; } set { _InventoryList = value; } }
    [SerializeField] private int _FloorNumberData;
    public int FloorNumberData { get { return _FloorNumberData; } set { _FloorNumberData = value; } }
}


