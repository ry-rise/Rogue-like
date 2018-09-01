using UnityEngine;

public abstract class MoveObject : MonoBehaviour {
    [SerializeField] private int _hp;
    [SerializeField] private int _level;
    [SerializeField] private int _exp;
    [SerializeField] private int _direction;
    private int State;
    public int HP { get { return _hp; } set { _hp = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int _Direction { get { return _direction; } set { _direction = value; } }
    enum Direction { UP, DOWN, LEFT, RIGHT }
    enum Action { standby,act_start,act,act_end,move_start,_moving,move_end,turn_end }
    protected virtual void Updating() { }
    protected virtual void Start()
    {

    }
    public abstract void Move();
}
