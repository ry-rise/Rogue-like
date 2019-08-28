using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class DataManager : MonoBehaviour {
    public static string GameFileName { get; private set; } = "//SaveData.json";
    public static string SettingFileName { get; private set; } = "//SettingsData.json";

    /// <summary>
    /// ゲームデータのセーブ
    /// </summary>
    public static void GameDataSave(Player player)
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
            DEF = player.DEF,
            Satiety=player.Satiety
        };
        string json = JsonUtility.ToJson(gameData);
        string path = $"{Application.persistentDataPath}{GameFileName}";
        Debug.Log(json);
        File.WriteAllText(path, json);
    }
    /// <summary>
    /// ゲームデータのロード
    /// </summary>
    /// <param name="player"></param>
    public static void GameDataLoad(Player player)
    {
        string path = $"{Application.persistentDataPath}{GameFileName}";
        string json = File.ReadAllText(path);
        GameData restoreData = JsonUtility.FromJson<GameData>(json);
        //player.inventoryList = restoreData.InventoryList;
        GameManager.SetFloorNumber(restoreData.FloorNumberData); //= restoreData.FloorNumberData;
        player.HP = restoreData.HP;
        player.MaxHP = restoreData.MaxHP;
        player.ATK = restoreData.ATK;
        player.Level = restoreData.Level;
        player.Exp = restoreData.Exp;
        player.Direction = restoreData.Direction;
        player.DEF = restoreData.DEF;
        player.Satiety = restoreData.Satiety;
    }
    /// <summary>
    /// データの削除
    /// </summary>
    public void GameDataDelete()
    {
    }
    public void SettingsDataSave()
    {

    }
    public void SettingsDataLoad()
    {

    }
}
