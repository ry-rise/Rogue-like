using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour {
    [HideInInspector] public GameObject playerObject;
    private Player player;
    private GameObject camPos;
    public List<GameObject> enemies1;
    public List<GameObject> items1;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] itemPrefab;
    public int FloorNumber { get; set; }
    public bool TurnPlayer = false;
    public bool TurnEnemy = false;
    private int playerRandomX;
    private int playerRandomY;
    private int enemyRandomX;
    private int enemyRandomY;
    private int itemRandomX;
    private int itemRandomY;
    private Transform enemyHolder;
    private Transform itemHolder;

    private void Awake()
    {
        enemyHolder = new GameObject("enemy").transform;
        itemHolder = new GameObject("item").transform;
        FloorNumber = 1;
        enemies1 = new List<GameObject>();
        items1 = new List<GameObject>();
        camPos = GameObject.Find("Main Camera");
        playerObject = Instantiate(playerPrefab);
    }
    private void Start()
    {
        //FLOORのところにランダムでPlayerを移動
        while (true)
        {
            playerRandomX = Random.Range(0, mapGenerator.MapWidth);
            playerRandomY = Random.Range(0, mapGenerator.MapHeight);
            if (mapGenerator.mapStatus[playerRandomX, playerRandomY]
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
        //ListにenemyPrefabを追加、生成
        for (int j = 0; j < 20; j += 1)
        {
            enemies1.Add(Instantiate(enemyPrefab[0], enemyHolder) as GameObject);
        }
        //ListにitemPrefabを追加、生成
        for(int k = 0; k < 20; k += 1)
        {
            items1.Add(Instantiate(itemPrefab[0], itemHolder) as GameObject);
        }
        //コメント
        //FLOORのところにenemyを移動
        for (int i = 0; i < 20; i += 1)
        {
            while (true)
            {
                enemyRandomX = Random.Range(0, mapGenerator.MapWidth);
                enemyRandomY = Random.Range(0, mapGenerator.MapHeight
);
                if (mapGenerator.mapStatus[enemyRandomX, enemyRandomY]
                   == (int)MapGenerator.STATE.FLOOR)
                {
                    enemies1[i].transform.position = new Vector2(enemyRandomX, enemyRandomY);
                    mapGenerator.mapStatus[enemyRandomX, enemyRandomY] = (int)MapGenerator.STATE.ENEMY;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //FLOORのところにitemを移動
        for(int it = 0; it < 20; it += 1)
        {
            while (true)
            {
                itemRandomX = Random.Range(0, mapGenerator.MapWidth);
                itemRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.mapStatus[itemRandomX, itemRandomY]
                    == (int)MapGenerator.STATE.FLOOR)
                {
                    items1[it].transform.position = new Vector2(itemRandomX, itemRandomY);
                    mapGenerator.mapStatus[itemRandomX, itemRandomY] = (int)MapGenerator.STATE.ITEM;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //プレイヤーのターン
        TurnPlayer = true;
    }
    private void Update()
    {
        //カメラの中心にプレイヤーがいる
        camPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                playerObject.transform.position.y,
                                                playerObject.transform.position.z-1);
        //プレイヤーの行動が終わったら
        if (TurnPlayer == false)
        {
            //敵の処理をする
            for (int i = 0; i < enemies1.Count - 1; i += 1)
            {
                Enemy1 Enemy1Script = enemies1[i].GetComponent<Enemy1>();
                Enemy1Script.MoveEnemy();
            }
            TurnPlayer = true;
        }
    }
    
}
