using UnityEngine;
using UnityEngine.SceneManagement;

//シーン切り替え
public sealed class SceneChanger : MonoBehaviour
{
    /// <summary>
    /// タイトル画面からゲーム画面に
    /// </summary>
    public void FromTitleToStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    /// <summary>
    /// タイトル画面から設定画面に
    /// </summary>
    public void FromTitleToSettings()
    {
        SceneManager.LoadScene("GameSettings");
    }
    /// <summary>
    /// タイトル画面から終了する
    /// </summary>
    public void FromTitleToExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    /// <summary>
    /// ゲーム画面からゲームオーバーに
    /// </summary>
    public void FromPlayToOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    /// <summary>
    /// ゲームオーバーからタイトル画面に
    /// </summary>
    public void FromOverToTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
}

