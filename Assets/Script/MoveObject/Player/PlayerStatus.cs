using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Player player;

    // Player.cs の private STATE _state を触れないので、
    // いったん Player 側の public property（state）を使う前提
    public MoveObject.STATE State
    {
        get => player.state;
        set => player.state = value;
    }

    public int Level
    {
        get => player.Level;
        set => player.Level = value;
    }

    public int HP
    {
        get => player.HP;
        set => player.HP = value;
    }

    public int MaxHP
    {
        get => player.MaxHP;
        set => player.MaxHP = value;
    }

    public int ATK
    {
        get => player.ATK;
        set => player.ATK = value;
    }

    public int DEF
    {
        get => player.DEF;
        set => player.DEF = value;
    }

    public int Exp
    {
        get => player.Exp;
        set => player.Exp = value;
    }

    public int NextExp
    {
        get => player.NextExp;
        set => player.NextExp = value;
    }

    public int Satiety
    {
        get => player.Satiety;
        set => player.Satiety = value;
    }

    public int MaxSatiety => player.MaxSatiety;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("[PlayerStatus] Player が同じGameObjectにいません");
            enabled = false;
        }
    }

    public void InitDefaultForNewGame()
    {
        Level = 1;
        HP = 100;
        ATK = 10;
        DEF = 5;
        MaxHP = HP;
        Satiety = MaxSatiety;

        State = MoveObject.STATE.NONE;
    }

    public void ApplyStateTurnEffects()
    {
        switch (State)
        {
            case MoveObject.STATE.NONE:
                break;

            case MoveObject.STATE.POISON:
                HP -= 1;
                if (player.TryReleaseState()) State = MoveObject.STATE.NONE;
                break;

            case MoveObject.STATE.PARALYSIS:
                if (player.TryReleaseState()) State = MoveObject.STATE.NONE;
                else
                    GameManager.Instance.turnManager = GameManager.TurnManager.PlayerEnd;
                break;
        }
    }

    public void ApplySatietyTurnEffects()
    {
        if (Satiety == 0) HP -= 1;
        else Satiety -= 1;
    }
    #region レベルアップ時の挙動
    public void LevelUp()
    {
        Level += 1;
        NextExp -= 1;
        ATK *= 2;
        DEF *= 2;
        Log.Instance?.LogTextWrite("レベルが上がった！");
    }
    #endregion
}