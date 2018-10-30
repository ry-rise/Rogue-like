﻿using System.Collections;
using UnityEngine;

public abstract class MoveObject : MonoBehaviour {
    protected GameManager gameManager;
    private int State;//状態
    protected bool funcEnd = false;
    #region プロパティ
    public int HP { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Direction { get; set; }
    #endregion
    enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    enum ACTION { TURN_STANDBY,ACT_START,ACT,ACT_END,MOVE_START,MOVING,MOVE_END,TURN_END }
    //private BoxCollider2D boxCollider;
    //private Rigidbody2D rigidbody2;
    public LayerMask HitLayer;
    
    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }    
}
