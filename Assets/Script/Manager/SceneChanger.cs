using UnityEngine;
using UnityEngine.SceneManagement;

//シーン切り替え
public static class SceneChanger
{
    /// <summary>
    /// タイトル画面に移動
    /// </summary>
    public static void ToTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
    /// <summary>
    /// ゲーム画面に移動
    /// </summary>
    public static void ToStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    /// <summary>
    /// 設定画面に移動
    /// </summary>
    public static void ToSettings()
    {
        SceneManager.LoadScene("GameSettings");
    }
    /// <summary>
    /// ゲームオーバー画面に移動
    /// </summary>
    public static void ToOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public static void ToExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
}

