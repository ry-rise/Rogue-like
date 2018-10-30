using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour {
    [HideInInspector] public GameObject playerObject;
    private Player player;
    private Enemy1 enemy1;
    private GameObject camPos;
    private GameObject enemyInstance;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrafab;
    [SerializeField] private GameObject[] enemyPrafab;
    public int Floor_number { get; set; }
    public bool TurnPlayer = false;
    public bool TurnEnemy = false;
    public List<GameObject> enemies1;
    private void Awake()
    {
        enemy1 = GetComponent<Enemy1>();
        Floor_number = 1;
        enemies1 = new List<GameObject>();
        camPos = GameObject.Find("Main Camera");
        playerObject = Instantiate(playerPrafab);
        enemyInstance = Instantiate(enemyPrafab[0],
                                    new Vector2(0, 0),
                                    Quaternion.identity);

        //turn_enemies = new List<bool>();
    }
    private void Start()
    {
        //FLOORのところにランダムでPlayerを生成
        //for (int x = 0; x > 1;x++) {
        //    if (mapGenerator.mapStatus[1, 1] == (int)MapGenerator.STATE.FLOOR)
        //    {
        //        Instantiate
        //    }
        //}
        enemies1.Add(enemyInstance);
        TurnPlayer = true;
    }
    private void Update()
    {
        camPos.transform.position = new Vector3(playerObject.transform.position.x,
                                                playerObject.transform.position.y,
                                                playerObject.transform.position.z-1);
        if (TurnPlayer == false)
        {

        }
    }
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
}
