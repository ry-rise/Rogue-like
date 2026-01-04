using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public sealed class DataManager //: MonoBehaviour 
{
    public static string GameFileName { get; private set; } = "//SaveData.json";
    public static string SettingsFileName { get; private set; } = "//SettingsData.json";

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
            Satiety = player.Satiety
        };
        string json = JsonUtility.ToJson(gameData);
        string path = $"{Application.persistentDataPath}{GameFileName}";
        //Debug.Log(json);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// ゲームデータのロード
    /// </summary>
    /// <param name="player"></param>
    public static void GameDataLoad(Player player)
    {
        string path = $"{Application.persistentDataPath}{GameFileName}";
        //初回起動（ファイル無し）でも落ちないようにする
        if (!File.Exists(path))
        {
            Debug.Log($"[DataManager] SaveData.json が無いので初期値で作成します: {path}");
            GameDataSave(player);
            return;
        }
        try
        {
            string json = File.ReadAllText(path);
            GameData restoreData = JsonUtility.FromJson<GameData>(json);
            //壊れてる/空の可能性もあるので念のため
            if (restoreData == null)
            {
                Debug.LogWarning("[DataManager] SaveData.json の読み込みに失敗。初期値で作り直します");
                GameDataSave(player);
                return;
            }
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
        catch (System.Exception e)
        {
            Debug.LogWarning($"[DataManager] SaveData.json 読み込み例外。初期値で作り直します: {e.Message}");
            GameDataSave(player);
        }
    }

    /// <summary>
    /// データの削除
    /// </summary>
    public static void GameDataDelete()
    {
        File.Delete($"{Application.persistentDataPath}{GameFileName}");
    }

    /// <summary>
    /// 設定データ保存
    /// </summary>
    /// <param name="audioMixer"></param>
    public static void SettingsDataSave(AudioMixer audioMixer)
    {
        SettingsDataSave(audioMixer, null);
    }

    public static void SettingsDataSave(AudioMixer audioMixer, InputActionAsset actions)
    {
        // 既存を読み込んで引き継ぐ（無ければ新規）
        SettingsData settingsData = LoadSettingsDataOrNull() ?? new SettingsData();

        // 音量（audioMixer がある時だけ更新）
        if (audioMixer != null)
        {
            float MasterVolume;
            float BGMVolume;
            float SEVolume;
            audioMixer.GetFloat("MasterVol", out MasterVolume);
            audioMixer.GetFloat("BGMVol", out BGMVolume);
            audioMixer.GetFloat("SEVol", out SEVolume);
            settingsData.MasterVolume = MasterVolume;
            settingsData.bgmVolume = BGMVolume;
            settingsData.seVolume = SEVolume;
        }

        // キーバインド（actions がある時だけ更新）
        if (actions != null)
        {
            settingsData.BindingOverridesJson = actions.SaveBindingOverridesAsJson();
        }

        WriteSettingsData(settingsData);
    }

    /// <summary>
    /// 設定データ読み込み
    /// </summary>
    /// <param name="audioMixer"></param>
    public static void SettingsDataLoad(AudioMixer audioMixer)
    {
        SettingsDataLoad(audioMixer, null);
    }

    public static void SettingsDataLoad(AudioMixer audioMixer, InputActionAsset actions)
    {
        string path = $"{Application.persistentDataPath}{SettingsFileName}";

        //初回起動（ファイル無し）でも落ちないようにする
        if (!File.Exists(path))
        {
            Debug.Log($"[DataManager] SettingsData.json が無いので初期値で作成します: {path}");
            SettingsDataSave(audioMixer, actions);// 現在のAudioMixer値を保存（=初期値扱い）
            return;
        }
        try
        {
            string json = File.ReadAllText(path);
            SettingsData restoreSettingsData = JsonUtility.FromJson<SettingsData>(json);

            //壊れてる/空の可能性もあるので念の為
            if (restoreSettingsData == null)
            {
                Debug.LogWarning("[DataManager] SettingsData.json の読み込みに失敗。初期値で作り直します");
                SettingsDataSave(audioMixer, actions);
                return;
            }

            //音量の復元
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MasterVol", restoreSettingsData.MasterVolume);
                audioMixer.SetFloat("BGMVol", restoreSettingsData.bgmVolume);
                audioMixer.SetFloat("SEVol", restoreSettingsData.seVolume);
            }

            // キーバインドの復元
            if (actions != null && !string.IsNullOrEmpty(restoreSettingsData.BindingOverridesJson))
            {
                actions.LoadBindingOverridesFromJson(restoreSettingsData.BindingOverridesJson);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[DataManager] SettingsData.json 読み込み例外。初期値で作り直します: {e.Message}");
            SettingsDataSave(audioMixer);
        }
    }

    private static SettingsData LoadSettingsDataOrNull()
    {
        string path = $"{Application.persistentDataPath}{SettingsFileName}";
        if (!File.Exists(path)) return null;

        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SettingsData>(json);
        }
        catch
        {
            return null;
        }
    }

    private static void WriteSettingsData(SettingsData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = $"{Application.persistentDataPath}{SettingsFileName}";
        File.WriteAllText(path, json);
    }

}
