using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region シーン切り替え
    public void SceneChange()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            //タイトル画面の時
            case "GameTitle":
                if (transform.name == "Button_Start")
                {
                    //（タイトル→ゲーム）
                    SceneManager.LoadScene("GamePlay");
                }
                if (transform.name == "Button_Settings")
                {
                    SceneManager.LoadScene("Settings");
                }
                if (transform.name == "Button_Exit")
                {
                    //終了
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
                break;
            //ゲーム画面の時
            case "GamePlay":
                SceneManager.LoadScene("GameOver");
                break;
            //ゲームオーバーになった時
            case "GameOver":
                SceneManager.LoadScene("GameTitle");
                break;
        }
    }
}
    #endregion

    