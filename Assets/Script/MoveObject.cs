﻿using UnityEngine;

public abstract class MoveObject : MonoBehaviour
{
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected SceneChanger sceneChanger;
    protected bool TurnEnd { get; set; } = false;
    public int HP { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Direction { get; set; }
    public int ATK { get; set; }
    public int DEF { get; set; }
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    public enum STATE { NONE, POISON, PARALYSIS }
    public enum MOVEPATTERN { }
    protected DIRECTION direction;

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mapGenerator = GameObject.Find("GameManager").GetComponent<MapGenerator>();
        sceneChanger = GameObject.Find("GameManager").GetComponent<SceneChanger>();
    }
    
    /// <summary>
    /// 状態異常の解除判定
    /// </summary>
    protected bool ReleaseDetermination()
    {
        if (Random.Range(0, 5) == 0)
        {
            return true;
        }
        return false;
    }
}
