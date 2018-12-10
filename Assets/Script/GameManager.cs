using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour {
    [HideInInspector] public GameObject playerObject;
    private Player player;
    private GameObject camPos;
    public List<GameObject> enemies1List;
    public List<GameObject> items1List;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] itemPrefab;
    public int FloorNumber { get; set; }
    public bool TurnPlayer = false;
    //public bool TurnEnemy = false;
    private Transform enemyHolder;
    private Transform itemHolder;

    private void Awake()
    {
        enemyHolder = new GameObject("enemy").transform;
        itemHolder = new GameObject("item").transform;
        FloorNumber = 1;
        enemies1List = new List<GameObject>();
        items1List = new List<GameObject>();
        camPos = GameObject.Find("Main Camera");
        playerObject = Instantiate(playerPrefab);
    }
    private void Start()
    {
        RandomDeploy();
        CameraOnCenter();
        //ListにenemyPrefabを追加、生成
        for (int j = 0; j < 20; j += 1)
        {
            enemies1List.Add(Instantiate(enemyPrefab[0], enemyHolder) as GameObject);
        }
        //ListにitemPrefabを追加、生成
        for(int k = 0; k < 20; k += 1)
        {
            items1List.Add(Instantiate(itemPrefab[0], itemHolder) as GameObject);
        }
        //プレイヤーのターン
        TurnPlayer = true;
    }
    private void Update()
    {
        //プレイヤーの行動が終わったら
        if (TurnPlayer == false)
        {
            //敵の処理をする
            for (int i = 0; i < enemies1List.Count; i += 1)
            {
                Enemy1 Enemy1Script = enemies1List[i].GetComponent<Enemy1>();
                Enemy1Script.MoveEnemy();
            }
            TurnPlayer = true;
        }
    }
    /// <summary>
    /// Player,Enemy,Itemを配置
    /// </summary>
    private void RandomDeploy()
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
        for (int i = 0; i < enemies1List.Count - 1; i += 1)
        {
            while (true)
            {
                int enemyRandomX = Random.Range(0, mapGenerator.MapWidth);
                int enemyRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[enemyRandomX, enemyRandomY]
                   == (int)MapGenerator.STATE.FLOOR)
                {
                    enemies1List[i].transform.position = new Vector2(enemyRandomX, enemyRandomY);
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
        for (int it = 0; it < items1List.Count - 1; it += 1)
        {
            while (true)
            {
                int itemRandomX = Random.Range(0, mapGenerator.MapWidth);
                int itemRandomY = Random.Range(0, mapGenerator.MapHeight);
                if (mapGenerator.MapStatusType[itemRandomX, itemRandomY]
                    == (int)MapGenerator.STATE.FLOOR)
                {
                    items1List[it].transform.position = new Vector2(itemRandomX, itemRandomY);
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
    /// カメラの中心にプレイヤーがいる
    /// </summary>
    public void CameraOnCenter()
    {
        camPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                playerObject.transform.position.y,
                                                playerObject.transform.position.z - 1);
    }
}
