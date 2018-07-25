using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            //シーン遷移（タイトル→ゲーム）
            SceneManager.LoadScene("GamePlay");
        }
    }
}
