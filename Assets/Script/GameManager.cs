using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour {
    [HideInInspector] public GameObject playerObject;
    private Player player;
    private GameObject camPos;
    public List<GameObject> enemies1;
    private List<GameObject> enemy1Script;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    public int FloorNumber { get; set; }
    public bool TurnPlayer = false;
    public bool TurnEnemy = false;
    private int playerRandomX;
    private int playerRandomY;
    private int enemyRandomX;
    private int enemyRandomY;
    private Transform enemyHolder;
    private void Awake()
    {

        enemyHolder = new GameObject("enemy").transform;
        FloorNumber = 1;
        enemies1 = new List<GameObject>();
        enemy1Script = new List<GameObject>();
        camPos = GameObject.Find("Main Camera");
        playerObject = Instantiate(playerPrefab);
    }
    private void Start()
    {
        //FLOORのところにランダムでPlayerを移動
        while (true)
        {
            playerRandomX = Random.Range(0, 80);
            playerRandomY = Random.Range(0, 80);
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
            enemies1.Add(Instantiate(enemyPrefab[0], enemyHolder)as GameObject);
        }
        //FLOORのところにenemyを移動
        for (int i = 0; i < 20; i += 1)
        {
            while (true)
            {
                enemyRandomX = Random.Range(0, 80);
                enemyRandomY = Random.Range(0, 80);
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
                //enemies1[i].
            }
            TurnPlayer = true;
        }
    }
    #region シーン切り替え
    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            if (transform.name == "Button_Start")
            {
                //（タイトル→ゲーム）
                SceneManager.LoadScene("GamePlay");
            }
            if (transform.name == "Button_Exit")
            {
                //終了
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            SceneManager.LoadScene("GameOver");
        }
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            SceneManager.LoadScene("GameTitle");
        }
    }
    #endregion
}
