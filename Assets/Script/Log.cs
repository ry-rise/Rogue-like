using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour 
{
	[SerializeField] private GameObject LogA;
	public static Text[] LogText{get;set;}
	public static void LogTextWrite(string str)
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
