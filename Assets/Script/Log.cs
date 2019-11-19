using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour 
{
    private static Log instance;
    public static Log Instance 
    { 
        get 
        { 
            if(instance==null)
            {
                GameObject obj=GameObject.Find("Log");
                instance=obj.GetComponent<Log>();
            }
            return instance; 
        } 
    }
    public Text[] LogText { get; set; }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LogText = new Text[5];
        LogText = GetComponentsInChildren<Text>();
    }
    private void OnDestroy()
    {
        Destroy(this);
    }
    public void LogTextWrite(string str)
    {
        for (int i = 0; i < LogText.Length; i += 1)
        {
            //ログが空だった場合
            if (string.IsNullOrEmpty(LogText[i].text) == true)
            {
                LogText[i].text = str;
                break;
            }
            else { continue; }
        }
        //すべてのログに文字が入っていた場合に一個ずつずらす
        if (string.IsNullOrEmpty(LogText[LogText.Length - 1].text) == false)
        {
            LogText[0].text = LogText[1].text;
            LogText[1].text = LogText[2].text;
            LogText[2].text = LogText[3].text;
            LogText[3].text = LogText[4].text;
            LogText[LogText.Length - 1].text = str;
        }
    }
}
