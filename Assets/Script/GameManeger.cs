﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour {
    [SerializeField] private Player player;
    [SerializeField] private MapGenerator mapGenerator;
	void Start () {
		
	}
	
	void Update () {
		
	}

    //シーン遷移
    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            //（タイトル→ゲーム）
            SceneManager.LoadScene("GamePlay");
        }
    }
}
