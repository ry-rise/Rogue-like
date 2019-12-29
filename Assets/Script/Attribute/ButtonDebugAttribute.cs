using System;
using UnityEngine;
using UnityEditor;

/// Inspector に GUI.Button を表示して、指定された関数を実行するための関数
/// 使用例 [Button("関数名" , "ボタン名")] public int 変数名;
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class ButtonDebugAttribute : PropertyAttribute
{
    public string buttonFunction;   // 関数名
    public string buttonName;   // ボタンに表示するテキスト

    /// ■■■コンストラクタ■■■
    public ButtonDebugAttribute(string _function, string _name)
    {
        buttonFunction = _function;
        buttonName = _name;
    }
}
