using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject obj = GameObject.Find("Manager");
                instance = obj.GetComponent<GameManager>();
            }
            return instance;
        }
    }
    [HideInInspector] public GameObject playerObject;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] itemPrefab;
    private Player player;
    private MapGenerator mapGenerator;
    private FadeManager fadeManager;
    public GameObject mainCamPos { get; set; }
    //private GameObject subCamPos;
    private Transform enemyHolder;
    private Transform itemHolder;
    public enum TurnManager { PlayerStart, PlayerMove, PlayerAttack, PlayerEnd, StateJudge, SatietyCheck, HierarchyMovement, EmemiesTurn, EmemiesEnd }
    [SerializeField] private TurnManager _turnManager;
    public TurnManager turnManager { get { return _turnManager; } set { _turnManager = value; } }
    public List<GameObject> enemiesList;
    public List<GameObject> itemsList;
    private static int FloorNumber { get; set; }
    public static int TotalScore { get; set; }

    public bool GamePause { get; set; } = false;

    private void Awake()
    {
        instance = this;
        Refrash();
        if (File.Exists($"{Application.persistentDataPath}{DataManager.GameFileName}") == false)
        {
            FloorNumber = 1;
        }
        mainCamPos = GameObject.Find("Main Camera");
        //subCamPos = GameObject.Find("Sub Camera");
        playerObject = Instantiate(playerPrefab);
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        mainCamPos.transform.parent = playerObject.transform;
    }
    public void Start()
    {
        ListAdd();
        RandomDeploy();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (File.Exists($"{Application.persistentDataPath}{DataManager.GameFileName}") == true)
        {
            Debug.Log("LOAD");
            DataManager.GameDataLoad(player);
        }
        //プレイヤーのターン
        turnManager = TurnManager.PlayerStart;


    }
    private void Update()
    {
        switch (turnManager)
        {
            //プレイヤーの行動が終わったら
            case TurnManager.PlayerEnd:

                if (player.isExit == true)
                {
                    turnManager = TurnManager.HierarchyMovement;
                }
                else
                {
                    turnManager = TurnManager.EmemiesTurn;

                    EnemiesAction<EnemyKnight>();
                    EnemiesAction<EnemyZombie>();
                    turnManager = TurnManager.EmemiesEnd;
                }
                break;
            //敵の行動が終わったら
            case GameManager.TurnManager.EmemiesEnd:
                player.isMoving=false;
                turnManager = TurnManager.PlayerStart;
                break;
                
            //階層移動
            case TurnManager.HierarchyMovement:
                FloorNumber += 1;
                DataManager.GameDataSave(player);
                SceneManager.LoadScene("FloorNumberView");
                break;
                
            default:
                break;
        }
    }
    private void LateUpdate()
    {
        mainCamPos.transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -1);
    }
    /// <summary>
    /// 敵の処理をする
    /// </summary>
    /// <typeparam name="T">敵のクラス</typeparam>
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
        DataManager.GameDataSave(player);
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
                mapGenerator.MapStatusMoveObject[playerRandomX, playerRandomY] = (int)MapGenerator.STATE.PLAYER;
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
                    mapGenerator.MapStatusMoveObject[enemyRandomX, enemyRandomY] = (int)MapGenerator.STATE.ENEMY;
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
        //subCamPos.transform.position = new Vector3(playerObject.transform.position.x,
        //                                           playerObject.transform.position.y,
        //                                           playerObject.transform.position.z - 15);
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
        player.isExit = false;
        fadeManager.isFadeOut = true;
        FloorNumber += 1;
        //FloorMoveManager.viewFloorNumber=FloorNumber;
        Destroy(GameObject.Find("Map"));
        Destroy(GameObject.Find("Enemy"));
        Destroy(GameObject.Find("Item"));
        mapGenerator.Awake();
        Refrash();
        ListAdd();
        RandomDeploy();
        fadeManager.isFadeIn = true;
        DataManager.GameDataSave(player);
    }
    
    public static int GetFloorNumber()
    {
        //if (File.Exists($"{Application.persistentDataPath}{FileName}"))
        //{
        //    DataLoad();
        //}
        return FloorNumber;
    }
    public static void SetFloorNumber(int FloorNumberData)
    {
        FloorNumber = FloorNumberData;
    }
    public static int GetTotalScore()
    {
        return 0;
    }
    IEnumerator KeyWait()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
    }
}
