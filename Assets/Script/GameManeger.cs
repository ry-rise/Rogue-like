using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class GameManeger : MonoBehaviour {
    private Player player;
    //private MapGenerator mapGenerator;
    public int floor_number = 1;
    public List<Enemy1> enemies1;
    public bool turn_player = false;
    public bool turn_enemy = false;
    public bool[] turn_enemies;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        turn_player = true;
    }
    //シーン遷移
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
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
}
