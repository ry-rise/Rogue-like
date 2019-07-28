using UnityEngine;
using UnityEngine.SceneManagement;

//シーン切り替え
public static class SceneChanger
{
    /// <summary>
    /// タイトル画面からゲーム画面に
    /// </summary>
    public static void FromTitleToStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    /// <summary>
    /// タイトル画面から設定画面に
    /// </summary>
    public static void FromTitleToSettings()
    {
        SceneManager.LoadScene("GameSettings");
    }
    /// <summary>
    /// 設定画面とからタイトル画面に
    /// </summary>
    public static void FromSettingsToTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
    /// <summary>
    /// タイトル画面から終了する
    /// </summary>
    public static void FromTitleToExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    /// <summary>
    /// ゲーム画面からゲームオーバーに
    /// </summary>
    public static void FromPlayToOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    /// <summary>
    /// ゲームオーバーからタイトル画面に
    /// </summary>
    public static void FromOverToTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
}

