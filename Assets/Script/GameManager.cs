using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour
{
    [HideInInspector] public GameObject playerObject;
    private Player player;
    private GameObject camPos;
    public List<GameObject> enemiesList;
    public List<GameObject> itemsList;
    private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] itemPrefab;
    public int FloorNumber { get; set; }
    public bool TurnPlayer { get; set; } = false;
    public bool GamePause { get; set; } = false;
    private Transform enemyHolder;
    private Transform itemHolder;
    public readonly string FileName = "//SaveData.json";

    private void Awake()
    {
        enemyHolder = new GameObject("enemy").transform;
        itemHolder = new GameObject("item").transform;
        FloorNumber = 1;
        enemiesList = new List<GameObject>();
        itemsList = new List<GameObject>();
        camPos = GameObject.Find("Main Camera");
        playerObject = Instantiate(playerPrefab);
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        if (File.Exists($"{Application.persistentDataPath}{FileName}") == true)
        {
            Debug.Log("LOAD");
            //DataLoad();
        }
    }
    public void Start()
    {
        ListAdd();
        RandomDeploy();
        CameraOnCenter();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //if (File.Exists($"{Application.persistentDataPath}{FileName}") == true)
        //{
        //    Debug.Log("LOAD");
        //    DataLoad();
        //}
        //プレイヤーのターン
        TurnPlayer = true;
    }
    private void Update()
    {
        //プレイヤーの行動が終わったら
        if (TurnPlayer == false)
        {
            //敵の処理をする
            for (int i = 0; i < enemiesList.Count; i += 1)
            {
                if(enemiesList[i].GetComponent<EnemyZombie>()!=null)
                {
                    EnemyZombie enemyZombie = enemiesList[i].GetComponent<EnemyZombie>();
                    enemyZombie.MoveEnemy((int)enemiesList[i].transform.position.x,
                                          (int)enemiesList[i].transform.position.y);
                }
                else if(enemiesList[i].GetComponent<EnemyKnight>()!=null)
                {
                    EnemyKnight enemyKnight = enemiesList[i].GetComponent<EnemyKnight>();
                    enemyKnight.MoveEnemy((int)enemiesList[i].transform.position.x,
                                          (int)enemiesList[i].transform.position.y);
                }
            }
            TurnPlayer = true;
        }
    }
    /// <summary>
    /// アプリ終了時に呼び出し
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("SAVE");
        //DataSave();
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
                break;
            }
            else
            {
                continue;
            }
        }
        //FLOORのところにenemyを移動
        for (int i = 0; i < enemiesList.Count - 1; i += 1)
        {
            while (true)
            {
                int enemyRandomX = Random.Range(0, mapGenerator.MapWidth);
                int enemyRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[enemyRandomX, enemyRandomY]
                   == (int)MapGenerator.STATE.FLOOR)
                {
                    enemiesList[i].transform.position = new Vector2(enemyRandomX, enemyRandomY);
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
        for (int it = 0; it < itemsList.Count - 1; it += 1)
        {
            while (true)
            {
                int itemRandomX = Random.Range(0, mapGenerator.MapWidth);
                int itemRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[itemRandomX, itemRandomY]
                    == (int)MapGenerator.STATE.FLOOR)
                {
                    itemsList[it].transform.position = new Vector2(itemRandomX, itemRandomY);
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
    /// カメラの中心にプレイヤーを配置
    /// </summary>
    public void CameraOnCenter()
    {
        camPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                playerObject.transform.position.y,
                                                playerObject.transform.position.z - 1);
    }
    /// <summary>
    /// Listに追加する
    /// </summary>
    private void ListAdd()
    {
        //ListにenemyPrefabを追加、生成
        for (int j = 0; j < 15; j += 1)
        {
            enemiesList.Add(Instantiate(enemyPrefab[Random.Range(0,enemyPrefab.Length)], enemyHolder) as GameObject);
        }
        //ListにitemPrefabを追加、生成
        for (int k = 0; k < 20; k += 1)
        {
            itemsList.Add(Instantiate(itemPrefab[0], itemHolder) as GameObject);
        }
    }
    /// <summary>
    /// セーブ
    /// </summary>
    private void DataSave()
    {
        GameData gameData = new GameData()
        {
            InventoryList = player.inventoryList,
            FloorNumberData = FloorNumber,
            HP=player.HP,
            ATK=player.ATK,
            Level=player.Level,
            Exp=player.Exp,
            Direction=player.Direction,
            DEF=player.DEF
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
        player.inventoryList = restoreData.InventoryList;
        FloorNumber = restoreData.FloorNumberData;
        player.HP=restoreData.HP;
        player.ATK=restoreData.ATK;
        player.Level=restoreData.Level;
        player.Exp=restoreData.Exp;
        player.Direction=restoreData.Direction;
        player.DEF=restoreData.DEF;
    }
}
