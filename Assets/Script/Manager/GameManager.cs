using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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
            if (instance == null)
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
    [SerializeField] private Log log;
    private Player player;
    private MapGenerator mapGenerator;
    private FadeManager fadeManager;
    public GameObject mainCamPos { get; set; }
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
            case TurnManager.EmemiesEnd:

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
        int maxTry = 5000;

        // -----------------------------
        // Player配置（床 かつ 未占有）
        // -----------------------------
        {
            int tries = 0;
            while (tries++ < maxTry)
            {
                int x = Random.Range(0, mapGenerator.MapWidth);
                int y = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[x, y] == (int)MapGenerator.STATE.FLOOR &&
                    mapGenerator.MapStatusMoveObject[x, y] == (int)MapGenerator.STATE.FLOOR)
                {
                    playerObject.transform.position = new Vector2(x, y);
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.PLAYER;
                    break;
                }
            }
        }

        int px = (int)playerObject.transform.position.x;
        int py = (int)playerObject.transform.position.y;

        //敵が近くに湧かない距離（マンハッタン距離）
        int minEnemyDist = 6;

        // -----------------------------
        // Enemy配置（床 かつ 未占有 かつ プレイヤーから距離）
        // -----------------------------
        foreach (var enemy in enemiesList)
        {
            int tries = 0;
            while (tries++ < maxTry)
            {
                int x = Random.Range(0, mapGenerator.MapWidth);
                int y = Random.Range(0, mapGenerator.MapHeight);

                int dist = Mathf.Abs(x - px) + Mathf.Abs(y - py);

                if (mapGenerator.MapStatusType[x, y] == (int)MapGenerator.STATE.FLOOR &&
                    mapGenerator.MapStatusMoveObject[x, y] == (int)MapGenerator.STATE.FLOOR &&
                    dist >= minEnemyDist)
                {
                    enemy.transform.position = new Vector2(x, y);
                    mapGenerator.MapStatusMoveObject[x, y] = (int)MapGenerator.STATE.ENEMY;
                    break;
                }
            }
        }

        // -----------------------------
        // Item配置（床 かつ 未占有 かつ まだItemじゃない）
        // ※ Itemは MapStatusType を ITEM にして管理してるので、それも考慮
        // -----------------------------
        foreach (var item in itemsList)
        {
            int tries = 0;
            while (tries++ < maxTry)
            {
                int x = Random.Range(0, mapGenerator.MapWidth);
                int y = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[x, y] == (int)MapGenerator.STATE.FLOOR &&
                    mapGenerator.MapStatusMoveObject[x, y] == (int)MapGenerator.STATE.FLOOR &&
                    mapGenerator.MapStatusItem[x, y] == (int)MapGenerator.ITEM_STATE.NONE)
                {
                    item.transform.position = new Vector2(x, y);
                    mapGenerator.MapStatusItem[x, y] = (int)MapGenerator.ITEM_STATE.ITEM;
                    break;
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

    public void TryPickupItemAt(int x, int y)
    {
        if (mapGenerator.MapStatusItem[x, y] == (int)MapGenerator.ITEM_STATE.NONE)
            return;

        // itemsListから座標一致のアクティブなアイテムを探す
        foreach (var go in itemsList)
        {
            if (go == null || !go.activeSelf) continue;

            var p = go.transform.position;
            if ((int)p.x == x && (int)p.y == y)
            {
                var item = go.GetComponent<ItemBase>();
                if (item != null)
                {
                    item.Collect();
                    return;
                }
            }
        }

        // フェイルセーフ：MapにITEMがあるのに実体が見つからない場合は掃除
        mapGenerator.MapStatusItem[x, y] = (int)MapGenerator.ITEM_STATE.NONE;
    }

}
