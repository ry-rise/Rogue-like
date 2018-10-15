using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloorText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text SatietyText;
    [SerializeField] private GameObject Inventory_screen;
    private Player player;
    private GameManeger gameManeger;

	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
	}
	void Update ()
    {
        LevelText.text = "LV:" + player.Level.ToString();
        FloorText.text = gameManeger.floor_number.ToString() + "F";
        HPText.text = "HP:" + player.HP.ToString();
        SatietyText.text = "空腹度:" + player.Satiety.ToString();
        Inventory();
    }

    //インベントリ
    void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch (Inventory_screen.activeSelf)
            {
                case true:
                    Inventory_screen.SetActive(false);
                    break;
                case false:
                    Inventory_screen.SetActive(true);
                    break;
            }
        }
    }

}
