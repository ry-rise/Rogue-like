using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloorText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text SatietyText;
    [SerializeField] private GameObject InventoryScreen;
    private Player player;
    private GameManager gameManager;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	void Update ()
    {
        LevelText.text = $"LV:{player.Level.ToString()}";
        FloorText.text = $"{gameManager.FloorNumber.ToString()}F";
        HPText.text = $"HP:{player.HP.ToString()}";
        SatietyText.text = $"空腹度:{player.Satiety.ToString()}";
        Inventory();
    }

    //インベントリ
    void Inventory()
    {
        //Iキーを押すとインベントリが表示/非表示
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch (InventoryScreen.activeSelf)
            {
                case true:
                    InventoryScreen.SetActive(false);
                    break;
                case false:
                    InventoryScreen.SetActive(true);
                    break;
            }
        }
    }
   
}
