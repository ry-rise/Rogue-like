using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManeger : MonoBehaviour {
    private Player player;
    private MapGenerator mapGenerator;
	void Start () {
		
	}
	
	void Update () {
		
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
