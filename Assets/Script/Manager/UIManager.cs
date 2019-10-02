using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryScreen;
    [SerializeField] private GameObject Header;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloorText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text SatietyText;
    [SerializeField] private Text StateText;
    private Player player;
    private bool checkTurn;
	private void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StateTextChanger();
    }
	private void Update ()
    {
        LevelText.text = $"LV:{player.Level.ToString()}";
        FloorText.text = $"{GameManager.GetFloorNumber().ToString()}F";
        HPText.text = $"HP:{player.HP.ToString()}/{player.MaxHP.ToString()}";
        SatietyText.text = $"空腹度:{player.Satiety.ToString()}";
        if (player != null)
        {
            if (GameManager.Instance.turnManager == GameManager.TurnManager.StateJudge)
            {
                StateTextChanger();
            }
        }
        
        //Iキーを押すとインベントリが表示/非表示
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch(InventoryScreen.activeSelf)
            {
                case true:
                    InventoryScreen.SetActive(false);
                    GameManager.Instance.GamePause = false;
                    Header.SetActive(true);
                    break;
                case false:
                    InventoryScreen.SetActive(true);
                    GameManager.Instance.GamePause = true;
                    Header.SetActive(false);
                    break;
            }
        }
    }
    private void StateTextChanger()
    {
        switch (player.state)
        {
            case MoveObject.STATE.NONE:
                StateText.text = "状態異常なし";
                break;
            case MoveObject.STATE.PARALYSIS:
                StateText.text = "麻痺";
                break;
            case MoveObject.STATE.POISON:
                StateText.text = "毒";
                break;
        }
    }
}
