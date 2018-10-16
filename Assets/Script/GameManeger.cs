using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManeger : MonoBehaviour {
    private Player player;
    private GameObject cam_pos;
    //[SerializeField] private MapGenerator mapGenerator;
    public int floor_number = 1;
    public List<Enemy1> enemies1;
    public bool turn_player = false;
    public bool turn_enemy = false;
    public bool[] turn_enemies;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        cam_pos = GameObject.Find("Main Camera");
        player = GameObject.Find("Player").GetComponent<Player>();
        turn_player = true;
    }
    private void Update()
    {
        cam_pos.transform.position = new Vector3(player.transform.position.x,
                                                 player.transform.position.y,
                                                 player.transform.position.z-1);
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
