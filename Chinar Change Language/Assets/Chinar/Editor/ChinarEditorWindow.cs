// ========================================================
// 描述：编辑器类 —— 用于更加便捷的生成数据，免去手动创建的烦恼
// 作者：Chinar 
// 创建时间：2018-12-28 16:33:47
// 版 本：1.0
// ========================================================
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChinarX.Tool;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ChinarEditorWindow : EditorWindow
{
    [SerializeField] public List<Text>         DatamationTexts = new List<Text>(); //自动处理的Text，数据化列表
    [SerializeField] public Text               FindTargetText;                     //目标Text
    protected               SerializedObject   _SerializedObject;                  //序列化对象 _ this
    protected               SerializedProperty _DatamationProperty;                //序列化属性
    protected               SerializedProperty _FindTargetProperty;                //目标对象
    public                  string             TargetIndex;                        //索引


    [MenuItem("Chinar/Tool Window")]
    static void ToolsWindow()
    {
        ChinarEditorWindow chinar = GetWindow<ChinarEditorWindow>(false, "Chinar Language");
        chinar.Show();
    }


    protected void OnEnable()
    {
        _SerializedObject   = new SerializedObject(this);                        //使用当前类初始化
        _DatamationProperty = _SerializedObject.FindProperty("DatamationTexts"); //获取当前类中可序列话的属性
        _FindTargetProperty = _SerializedObject.FindProperty("FindTargetText");  //目标Text属性
    }


    void OnGUI()
    {
        #region 文本

        _SerializedObject.Update();   //更新
        EditorGUI.BeginChangeCheck(); //开始检查是否有修改
        GUILayout.Label("所有被数据化的 Text 列表");
        EditorGUILayout.PropertyField(_DatamationProperty, true); //第二个参数必须为true，否则无法显示子节点即List内容
        if (EditorGUI.EndChangeCheck())                           //结束检查是否有修改
        {
            _SerializedObject.ApplyModifiedProperties(); //提交修改
        }

        if (GUILayout.Button("数据化语言包"))
        {
            DatamationTexts.Clear();
            _DatamationProperty.ClearArray();
            LanguageData root   = new LanguageData();
            var          resAll = Resources.FindObjectsOfTypeAll<Text>();
            for (int i = 0; i < resAll.Length; i++)
            {
                DatamationTexts.Add(resAll[i]);
                ChinarRecordText crt = resAll[i].GetComponent<ChinarRecordText>() == null ? resAll[i].gameObject.AddComponent<ChinarRecordText>() : resAll[i].GetComponent<ChinarRecordText>();
                crt.I = i;
                if (crt.C == string.Empty) crt.C = crt.GetComponent<Text>().text;
                new ChinarRecordText(i, crt.E, crt.C, resAll[i].color, ref root.Datas);
            }

            Chinar.CreateXml(Application.streamingAssetsPath + "/" + "Language.chinar", root);
            _DatamationProperty = _SerializedObject.FindProperty("DatamationTexts"); //重新给Texts赋值
            _FindTargetProperty = _SerializedObject.FindProperty("FindTargetText");  //为了解决精确查找报错为空的情况
            AssetDatabase.Refresh();
        }

        #endregion
        GUILayout.Label("输入索引：精确查找对应 Text");
        EditorGUILayout.PropertyField(_FindTargetProperty, false);
        TargetIndex = GUILayout.TextField(TargetIndex);
        if (GUILayout.Button("开始查找"))
        {
            if (!Regex.IsMatch(TargetIndex, "^[0-9]*$"))
            {
                ShowNotification(new GUIContent("索引是阿拉伯数字，不是其他格式！"));
                return;
            }

            if (TargetIndex == string.Empty)
            {
                TargetIndex = 0.ToString();
                ShowNotification(new GUIContent("不能为空！"));
                return;
            }

            var num = int.Parse(TargetIndex);
            if (num >= _DatamationProperty.arraySize)
            {
                ShowNotification(new GUIContent("你要搜索的Text已超出太阳系！\n请检查索引，重新输入！"));
                return;
            }

            if (num < 0)
            {
                ShowNotification(new GUIContent("你给我解释解释，负数是什么鬼！？"));
                return;
            }

            _FindTargetProperty = _DatamationProperty.GetArrayElementAtIndex(num);
        }
    }
}