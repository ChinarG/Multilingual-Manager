// ========================================================
// 描述：编辑器类 —— 用于更加便捷的生成数据，免去手动创建的烦恼
// 作者：Chinar 
// 创建时间：2018-12-28 16:33:47
// 版 本：1.0
// ========================================================
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChinarX.Plugin.Language;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



namespace ChinarX.Window
{
    public class ChinarToolLanguageWindow : EditorWindow
    {
        [SerializeField] public List<Text>         DatamationTexts = new List<Text>();
        [SerializeField] public Text               FindTargetText;
        protected               SerializedObject   _SerializedObject;
        protected               SerializedProperty _DatamationProperty;
        protected               SerializedProperty _FindTargetProperty;
        public                  string             TargetIndex;


        [MenuItem("Chinar/Tool Window")]
        static void ToolsWindow()
        {
            ChinarToolLanguageWindow chinar = GetWindow<ChinarToolLanguageWindow>(false, "Chinar Language");
            chinar.Show();
        }


        protected void OnEnable()
        {
            _SerializedObject   = new SerializedObject(this);
            _DatamationProperty = _SerializedObject.FindProperty("DatamationTexts");
            _FindTargetProperty = _SerializedObject.FindProperty("FindTargetText");
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

                ChinarX.Chinar.CreateXml(Application.streamingAssetsPath + "/" + "Language.chinar", root);
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
}