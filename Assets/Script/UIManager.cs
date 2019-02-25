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
    private Text LevelText;
    private Text FloorText;
    private Text HPText;
    private Text SatietyText;
    private Text StateText;
    private Player player;
    private GameManager gameManager;

	private void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LevelText = LevelObject.GetComponent<Text>();
        FloorText = FloorObject.GetComponent<Text>();
        HPText = HPObject.GetComponent<Text>();
        SatietyText = SatietyObject.GetComponent<Text>();
        StateText = StateObject.GetComponent<Text>();
    }
	private void Update ()
    {
        LevelText.text = $"LV:{player.Level.ToString()}";
        FloorText.text = $"{gameManager.FloorNumber.ToString()}F";
        HPText.text = $"HP:{player.HP.ToString()}/{player.MaxHP.ToString()}";
        SatietyText.text = $"空腹度:{player.Satiety.ToString()}";
        switch(player.state)
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
            default:
                StateText.text = "aaa";
                break;
        }
        TextDisplay();
        
        //Iキーを押すとインベントリが表示/非表示
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryScreen.activeSelf == true)
            {
                InventoryScreen.SetActive(false);
                gameManager.GamePause = false;
            }
            else if (InventoryScreen.activeSelf == false)
            {
                InventoryScreen.SetActive(true);
                gameManager.GamePause = true;
            }
        }
    }

    //インベントリ
    private void Inventory() { }

    /// <summary>
    /// インベントリの表示/非表示でテキストの表示/非表示を切り替える
    /// </summary>
    private void TextDisplay()
    {
        if (gameManager.GamePause == true)
        {
            
        }
        else if(gameManager.GamePause==false)
        {
            LevelObject.SetActive(true);
            FloorObject.SetActive(true);
            HPObject.SetActive(true);
            SatietyObject.SetActive(true);
        }
    }
}
