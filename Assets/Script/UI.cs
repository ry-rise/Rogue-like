using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloorText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text SatietyText;
    private Player player;
    private GameManeger gameManeger;
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
	}
	
	void Update () {
        LevelText.text = "LV:" + player.Level.ToString();
        FloorText.text = gameManeger.floor_number.ToString() + "F";
        HPText.text = "HP:" + player.HP.ToString();
        SatietyText.text = "満腹度:" + player.Satiety.ToString();
    }

}
