using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class DataManager : MonoBehaviour {
    private Player player;
    //private GameManager gameManager;
    private readonly string GameFileName = "//SaveData.json";
    private readonly string SettingFileName = "//SettingsData.json";
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
    }
    /// <summary>
    /// セーブ
    /// </summary>
    public void GameDataSave()
    {
        GameData gameData = new GameData()
        {
            //InventoryList = player.inventoryList,
            FloorNumberData = GameManager.GetFloorNumber(),
            HP = player.HP,
            MaxHP = player.MaxHP,
            ATK = player.ATK,
            Level = player.Level,
            Exp = player.Exp,
            Direction = player.Direction,
            DEF = player.DEF
        };
        string json = JsonUtility.ToJson(gameData);
        string path = $"{Application.persistentDataPath}{GameFileName}";
        Debug.Log(json);
        File.WriteAllText(path, json);
    }
    public void GameDataLoad()
    {

    }
    public void SettingsDataSave()
    {

    }
    public void SettingsDataLoad()
    {

    }
}
