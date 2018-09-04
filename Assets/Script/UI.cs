using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloarText;
    [SerializeField] private Text SatietyText;
    private Player player;
    private GameManeger gameManeger;
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
	}
	
	void Update () {
        LevelText.text = "LV:" + player.Level.ToString();
        FloarText.text = gameManeger.floor_number.ToString() + "F";
        SatietyText.text = "満腹度:" + player.Satiety.ToString();
    }

}
