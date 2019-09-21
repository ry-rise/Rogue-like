using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelObject;
    [SerializeField] private GameObject FloorObject;
    [SerializeField] private GameObject HPObject;
    [SerializeField] private GameObject SatietyObject;
    [SerializeField] private GameObject StateObject;
    [SerializeField] private GameObject InventoryScreen;
    [SerializeField] private GameObject Log;
    [SerializeField] private GameObject Header;
    private Text LevelText;
    private Text FloorText;
    private Text HPText;
    private Text SatietyText;
    private Text StateText;
    public Text[] LogText { get; private set; }
    private Player player;
    private GameManager gameManager;
    private bool checkTurn;
	private void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        LogText = new Text[5];
        LevelText = LevelObject.GetComponent<Text>();
        FloorText = FloorObject.GetComponent<Text>();
        HPText = HPObject.GetComponent<Text>();
        SatietyText = SatietyObject.GetComponent<Text>();
        StateText = StateObject.GetComponent<Text>();
        LogText = Log.GetComponentsInChildren<Text>();
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
            if (gameManager.turnManager == GameManager.TurnManager.StateJudge)
            {
                StateTextChanger();
            }
        }
        
        //Iキーを押すとインベントリが表示/非表示
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryScreen.activeSelf == true)
            {
                InventoryScreen.SetActive(false);
                gameManager.GamePause = false;
                Header.SetActive(true);
            }
            else if (InventoryScreen.activeSelf == false)
            {
                InventoryScreen.SetActive(true);
                gameManager.GamePause = true;
                Header.SetActive(false);
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
