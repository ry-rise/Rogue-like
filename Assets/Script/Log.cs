using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private static Log instance;
    public static Log Instance
    {
        get
        {
            //既にキャッシュがあれば返す
            if (instance != null) return instance;
            //シーン内からLogオブジェクトを探す
            instance = FindFirstObjectByType<Log>();
            return instance;
        }
    }
    public Text[] LogText { get; set; }
    private bool isQuitting = false;
    private void Awake()
    {
        // 二重生成対策（必要なら）
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //取得
        LogText = GetComponentsInChildren<Text>();
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    public void LogTextWrite(string str)
    {
        //終了中は何もしない
        if (isQuitting) return;
        if (LogText == null || LogText.Length == 0) return;
        //空いているログ欄を探す
        for (int i = 0; i < LogText.Length; i += 1)
        {
            if (LogText[i] == null) continue;
            if (!LogText[i].gameObject.activeInHierarchy) continue;
            //ログが空だった場合
            if (string.IsNullOrEmpty(LogText[i].text))
            {
                LogText[i].text = str;
                break;
            }
        }
        //すべてのログに文字が入っていた場合に一個ずつずらす
        for (int i = 0; i < LogText.Length-1; i += 1)
        {
            if (LogText[i] == null||LogText[i+1] == null) continue;
            LogText[i].text = LogText[i + 1].text;
        }
        LogText[LogText.Length - 1].text = str;
    }
}
