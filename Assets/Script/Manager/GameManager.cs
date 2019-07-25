using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour
{
    [HideInInspector] public GameObject playerObject;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] itemPrefab;
    private Player player;
    private MapGenerator mapGenerator;
    private FadeManager fadeManager;
    private GameObject mainCamPos;
    private GameObject subCamPos;
    private Transform enemyHolder;
    private Transform itemHolder;
    public enum TurnManager { PLAYER_START, PLAYER_MOVE, PLAYER_ATTACK, PLAYER_END, STATE_JUDGE, SATIETY_CHECK, ENEMIES_TURN, ENIMIES_END }
    [SerializeField] private TurnManager _turnManager;
    public TurnManager turnManager { get { return _turnManager; } set { _turnManager = value; } }
    public List<GameObject> enemiesList;
    public List<GameObject> itemsList;
    public int FloorNumber { get; set; }
    public bool GamePause { get; set; } = false;
    public readonly string FileName = "//SaveData.json";

    private void Awake()
    {
        Refrash();
        if (File.Exists($"{Application.persistentDataPath}{FileName}") == false)
        {
            FloorNumber = 1;
        }
        mainCamPos = GameObject.Find("Main Camera");
        subCamPos = GameObject.Find("Sub Camera");
        playerObject = Instantiate(playerPrefab);
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        fadeManager = GameObject.Find("Canvas/FadeInOut").GetComponent<FadeManager>();
    }
    public void Start()
    {
        ListAdd();
        RandomDeploy();
        CameraOnCenter();
        Debug.Log(Application.persistentDataPath);
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (File.Exists($"{Application.persistentDataPath}{FileName}") == true)
        {
            Debug.Log("LOAD");
            DataLoad();
        }
        //プレイヤーのターン
        turnManager = TurnManager.PLAYER_START;

    }
    private void Update()
    {
        //プレイヤーの行動が終わったら
        if (turnManager == TurnManager.PLAYER_END)
        {
            turnManager = TurnManager.ENEMIES_TURN;

            //敵の処理をする
            EnemiesAction<EnemyKnight>();
            EnemiesAction<EnemyZombie>();
            turnManager = TurnManager.ENIMIES_END;
        }
    }
    private void EnemiesAction<T>()
        where T : EnemyBase
    {
        foreach (var enemy in enemiesList)
        {
            if (enemy.GetComponent<T>() != null)
            {
                T EnemyClass = enemy.GetComponent<T>();
                EnemyClass.MoveEnemy((int)enemy.transform.position.x, (int)enemy.transform.position.y);
                EnemyClass.AttackEnemy((int)enemy.transform.position.x, (int)enemy.transform.position.y);
            }
        }
    }
    /// <summary>
    /// アプリ終了時に呼び出し
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("SAVE_OnApplicationQuit");
        DataSave();
    }
    /// <summary>
    /// Player,Enemy,Itemを配置
    /// </summary>
    public void RandomDeploy()
    {
        //FLOORのところにランダムでPlayerを配置
        while (true)
        {
            int playerRandomX = Random.Range(0, mapGenerator.MapWidth);
            int playerRandomY = Random.Range(0, mapGenerator.MapHeight);
            if (mapGenerator.MapStatusType[playerRandomX, playerRandomY]
                == (int)MapGenerator.STATE.FLOOR)
            {
                playerObject.transform.position = new Vector2(playerRandomX, playerRandomY);
                mapGenerator.MapStatusType[playerRandomX, playerRandomY] = (int)MapGenerator.STATE.PLAYER;
                break;
            }
            else
            {
                continue;
            }
        }
        //FLOORのところにenemyを移動
        foreach (var enemy in enemiesList)
        {
            while (true)
            {
                int enemyRandomX = Random.Range(0, mapGenerator.MapWidth);
                int enemyRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[enemyRandomX, enemyRandomY]
                   == (int)MapGenerator.STATE.FLOOR)
                {
                    enemy.transform.position = new Vector2(enemyRandomX, enemyRandomY);
                    mapGenerator.MapStatusType[enemyRandomX, enemyRandomY] = (int)MapGenerator.STATE.ENEMY;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //FLOORのところにitemを移動
        foreach (var item in itemsList)
        {
            while (true)
            {
                int itemRandomX = Random.Range(0, mapGenerator.MapWidth);
                int itemRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[itemRandomX, itemRandomY]
                    == (int)MapGenerator.STATE.FLOOR)
                {
                    item.transform.position = new Vector2(itemRandomX, itemRandomY);
                    mapGenerator.MapStatusType[itemRandomX, itemRandomY] = (int)MapGenerator.STATE.ITEM;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }
    /// <summary>
    /// プレイヤーをカメラの中心に配置
    /// </summary>
    public void CameraOnCenter()
    {
        mainCamPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                    playerObject.transform.position.y,
                                                    playerObject.transform.position.z - 1);
        subCamPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                   playerObject.transform.position.y,
                                                   playerObject.transform.position.z - 15);
    }
    /// <summary>
    /// Listに追加する
    /// </summary>
    public void ListAdd()
    {
        //ListにenemyPrefabを追加、生成
        for (int j = 0; j < 15; j += 1)
        {
            enemiesList.Add(Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], enemyHolder) as GameObject);
        }
        //ListにitemPrefabを追加、生成
        for (int k = 0; k < 15; k += 1)
        {
            itemsList.Add(Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], itemHolder) as GameObject);
        }
    }
    /// <summary>
    /// 次の階層に行く時に呼ぶ
    /// </summary>
    public void Refrash()
    {
        enemyHolder = new GameObject("Enemy").transform;
        itemHolder = new GameObject("Item").transform;
        enemiesList = new List<GameObject>();
        itemsList = new List<GameObject>();
    }
    public void Exit()
    {
        fadeManager.isFadeOut = true;
        FloorNumber += 1;
        Destroy(GameObject.Find("Map"));
        Destroy(GameObject.Find("Enemy"));
        Destroy(GameObject.Find("Item"));
        mapGenerator.Awake();
        Refrash();
        ListAdd();
        RandomDeploy();
        CameraOnCenter();
        fadeManager.isFadeIn = true;
        DataSave();
    }
    /// <summary>
    /// セーブ
    /// </summary>
    private void DataSave()
    {
        GameData gameData = new GameData()
        {
            //InventoryList = player.inventoryList,
            FloorNumberData = FloorNumber,
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
        string path = $"{Application.persistentDataPath}{FileName}";
        Debug.Log(json);
        File.WriteAllText(path, json);
    }
    /// <summary>
    /// ロード
    /// </summary>
    private void DataLoad()
    {
        string path = $"{Application.persistentDataPath}{FileName}";
        string json = File.ReadAllText(path);
        GameData restoreData = JsonUtility.FromJson<GameData>(json);
        //player.inventoryList = restoreData.InventoryList;
        FloorNumber = restoreData.FloorNumberData;
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
    public void DataDelete()
    {
    }
    //デバッグ時のみ
    /*[System.Diagnostics.Conditional("a")]
    private static void A()
    {
        Debug.Log("a");
    }
    */
    IEnumerator KeyWait()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
    }
}
