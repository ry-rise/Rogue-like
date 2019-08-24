using UnityEngine;
using UnityEngine.SceneManagement;

//シーン切り替え
public static class SceneChanger
{
    /// <summary>
    /// タイトル画面からゲーム画面に
    /// </summary>
    public static void ToStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    /// <summary>
    /// タイトル画面から設定画面に
    /// </summary>
    public static void ToSettings()
    {
        SceneManager.LoadScene("GameSettings");
    }
    /// <summary>
    /// タイトル画面に移動
    /// </summary>
    public static void ToTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
    /// <summary>
    /// タイトル画面から終了する
    /// </summary>
    public static void ToExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    /// <summary>
    /// ゲーム画面からゲームオーバーに
    /// </summary>
    public static void ToOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}

