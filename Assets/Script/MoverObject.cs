using UnityEngine;

public abstract class MoverObject : MonoBehaviour {
    [SerializeField] private int _hp;
    [SerializeField] private int _level;
    [SerializeField] private int _exp;
    [SerializeField] private int _direction;
    public int HP { get { return _hp; } set { _hp = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int _Direction { get { return _direction; } set { _direction = value; } }
    enum Direction { UP, DOWN, LEFT, RIGHT }
    public abstract void Move();
}
