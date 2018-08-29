﻿using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManeger : MonoBehaviour {
    public static GameManeger gameManeger;
    private Player player;
    public MapGenerator mapGenerator;

    //private void Awake()
    //{
    //    if (gameManeger == null)
    //    {
    //        gameManeger = this;
    //    }
    //    else if (gameManeger!=this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);

    //    mapGenerator = GetComponent<MapGenerator>();
    //    mapGenerator.Awake();
    //}
    
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
