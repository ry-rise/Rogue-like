using UnityEngine;

public abstract class MoveObject : MonoBehaviour {
    protected GameManager gameManager;
    protected MapGenerator mapGenerator;
    protected SceneChanger sceneChanger;
    private int State;//状態
    protected bool TurnEnd { get; set; } = false;
    public int HP { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Direction { get; set; }
    public int ATK { get; set; }
    protected enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    protected enum STATE { NONE, POISON, PARALYSIS }
    
    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mapGenerator = GameObject.Find("GameManager").GetComponent<MapGenerator>();
    }    
}
