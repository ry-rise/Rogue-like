using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger : MonoBehaviour {
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
