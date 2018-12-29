// ========================================================
// 描述：
// 作者：Chinar 
// 创建时间：2018-12-28 16:33:47
// 版 本：1.0
// ========================================================
using System.Collections.Generic;
using ChinarX.Tool;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class ChinarEditorWindow : EditorWindow
{
    public static string CustomName    = "CustomName";
    public        int    ChinarNum     = 0;
    public static string ResourcesPath = "ResourcesInteriorPath";


    [MenuItem("Chinar/显示工具窗口")]
    static void 显示新窗口()
    {
        ChinarEditorWindow chinar = GetWindow<ChinarEditorWindow>(false, "Chinar 工具");
        chinar.Show();
    }


    [MenuItem("Chinar/Creat Drawing Data File")]
    static void CreateDrawingDatasFiles()
    {
    }


    [SerializeField] public List<Text> Texts = new List<Text>();



    [SerializeField] public Text Targete;


    //序列化对象
    protected SerializedObject _serializedObject;

    //序列化属性
    protected SerializedProperty _assetLstProperty;
    protected SerializedProperty _targetProperty;


    protected void OnEnable()
    {
        //使用当前类初始化
        _serializedObject = new SerializedObject(this);
        //获取当前类中可序列话的属性
        _assetLstProperty = _serializedObject.FindProperty("Texts");
        _targetProperty = _serializedObject.FindProperty("Targete");

    }


    void OnGUI()
    {
        #region 文本

        //更新
        _serializedObject.Update();

        //开始检查是否有修改
        EditorGUI.BeginChangeCheck();
        //第二个参数必须为true，否则无法显示子节点即List内容
        EditorGUILayout.PropertyField(_assetLstProperty, true);
        EditorGUILayout.PropertyField(_targetProperty, false);

        //结束检查是否有修改
        if (EditorGUI.EndChangeCheck())
        {
            //提交修改
            _serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("保存包数据"))
        {
            LanguageData root = new LanguageData();

            _assetLstProperty.ClearArray();
            Texts.Clear();
            var resAll = Resources.FindObjectsOfTypeAll<Text>();
            for (int i = 0; i < resAll.Length; i++)
            {
                //Debug.Log(resAll[i].name);
                Texts.Add(resAll[i]);
                ChinarRecordText uiChinarText = resAll[i].GetComponent<ChinarRecordText>() == null ? resAll[i].gameObject.AddComponent<ChinarRecordText>() : resAll[i].GetComponent<ChinarRecordText>();
                uiChinarText.L = i;
                new ChinarRecordText(i, uiChinarText.E, uiChinarText.C, resAll[i].color, ref root.Datas);
            }
            Chinar.CreateXml(Application.streamingAssetsPath + "/"+"Language.chinar", root);
            _assetLstProperty = _serializedObject.FindProperty("Texts");
            for (int i = 0; i < _assetLstProperty.arraySize; i++)
            {
                Debug.Log(_assetLstProperty.arrayElementType);
                Debug.Log(_assetLstProperty.stringValue);
                Debug.Log(_assetLstProperty.intValue);
                Debug.Log(_assetLstProperty.objectReferenceValue);
                Debug.Log(_assetLstProperty.arrayElementType);
                Debug.Log(_assetLstProperty.arrayElementType);
                Debug.Log(_assetLstProperty.arrayElementType);
            }
        }

        #endregion


        
        GUILayout.Label("精确查找对应文本");
        CustomName = GUILayout.TextField(CustomName);
        if (GUILayout.Button("查找"))
        {

            _targetProperty = _assetLstProperty.GetArrayElementAtIndex(int.Parse(CustomName));
            //if (CustomName==)
            //{

            //}

            //if (Selection.gameObjects.Length <= 0)
            //{
            //    ShowNotification(new GUIContent("请选择对象后，再执行操作"));
            //}
            //else
            //{
            //    for (int i = 0; i < Selection.transforms.Length; i++)
            //    {
            //        Undo.RecordObjects(Selection.gameObjects as GameObject[], "ChinarWindow_GameObject[]");
            //        Selection.transforms[i].SetSiblingIndex(i);
            //        Selection.transforms[i].name = CustomName + i;
            //    }
            //}
        }

    }


}