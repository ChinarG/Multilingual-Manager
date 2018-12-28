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

    //序列化对象
    protected SerializedObject _serializedObject;

    //序列化属性
    protected SerializedProperty _assetLstProperty;


    protected void OnEnable()
    {
        //使用当前类初始化
        _serializedObject = new SerializedObject(this);
        //获取当前类中可序列话的属性
        _assetLstProperty = _serializedObject.FindProperty("Texts");
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

        //结束检查是否有修改
        if (EditorGUI.EndChangeCheck())
        {
            //提交修改
            _serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("保存包数据"))
        {
            List<UiChinarTextData> vvv = new List<UiChinarTextData>();
            Root root = new Root();

            _assetLstProperty.ClearArray();
            Texts.Clear();
            var resAll = Resources.FindObjectsOfTypeAll<Text>();
            for (int i = 0; i < resAll.Length; i++)
            {
                //Debug.Log(resAll[i].name);
                Texts.Add(resAll[i]);
                UiChinarText uiChinarText = resAll[i].GetComponent<UiChinarText>() == null ? resAll[i].gameObject.AddComponent<UiChinarText>() : resAll[i].GetComponent<UiChinarText>();
                uiChinarText.L = i;
                new UiChinarText(i, uiChinarText.E, uiChinarText.C, resAll[i].color, ref root.Datas);
            }
            Chinar.CreateXml(Application.streamingAssetsPath + "/"+"Language.chinar", root.Datas);
            _assetLstProperty = _serializedObject.FindProperty("Texts");
        }

        #endregion
        GUILayout.Label("Chinar 工具栏");
        CustomName = GUILayout.TextField(CustomName);
        if (GUILayout.Button("修改所有对象名称并排序"))
        {
            if (Selection.gameObjects.Length <= 0)
            {
                ShowNotification(new GUIContent("请选择对象后，再执行操作"));
            }
            else
            {
                for (int i = 0; i < Selection.transforms.Length; i++)
                {
                    Undo.RecordObjects(Selection.gameObjects as GameObject[], "ChinarWindow_GameObject[]");
                    Selection.transforms[i].SetSiblingIndex(i);
                    Selection.transforms[i].name = CustomName + i;
                }
            }
        }

        CustomName = GUILayout.TextField(CustomName);
        ChinarNum  = int.Parse(GUILayout.TextField(ChinarNum.ToString()));
        if (GUILayout.Button("创建多个空物体"))
        {
            if (ChinarNum <= 0)
            {
                ShowNotification(new GUIContent("请写入创建空物体的数量"));
            }
            else
            {
                for (int i = 0; i < ChinarNum; i++)
                {
                    Undo.RegisterCreatedObjectUndo(new GameObject(CustomName), "Chinar Create gameobject");
                }
            }
        }

        ResourcesPath = GUILayout.TextField(ResourcesPath);
        if (GUILayout.Button("批量创建预设物"))
        {
            if (Selection.gameObjects.Length <= 0)
            {
                ShowNotification(new GUIContent("请选择对象后，再执行操作"));
            }
            else
            {
                BatchCreatePrefab();
            }
        }

        if (GUILayout.Button("去除BoxClider+存为预设物"))
        {
            if (Selection.gameObjects.Length <= 0)
            {
                ShowNotification(new GUIContent("请选择对象后，再执行操作"));
            }
            else
            {
                EditorUtility.DisplayProgressBar("去除进度", "0/" + Selection.gameObjects.Length + "完成修改", 1);
                int count = 0;
                for (var i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var t = Selection.gameObjects[i];
                    Undo.RecordObjects((GameObject[]) Selection.gameObjects, "ChinarWindow_DelegateBoxClider[]");
                    var bs = t.GetComponentsInChildren<BoxCollider>();
                    for (var j = 0; j < bs.Length; j++)
                    {
                        var b = bs[j];
                        if (!b.gameObject.name.Contains("."))
                        {
                            DestroyImmediate(b);
                        }
                    }

                    count++;
                    EditorUtility.DisplayProgressBar("修改进度", count + "/" + Selection.gameObjects.Length + "完成修改", progress: count / Selection.gameObjects.Length);
                }

                EditorUtility.ClearProgressBar();
                BatchCreatePrefab();
            }
        }
    }


    private void BatchCreatePrefab()
    {
        EditorUtility.DisplayProgressBar("修改进度", "0/" + Selection.gameObjects.Length + "完成修改", 1);
        int count = 0;
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Undo.RecordObjects((GameObject[]) Selection.gameObjects, "ChinarWindow_DelegateSpace[]");
            Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/" + ResourcesPath + "/" + Selection.gameObjects[i].name + ".prefab");
            PrefabUtility.ReplacePrefab(Selection.gameObjects[i], tempPrefab);
            count++;
            EditorUtility.DisplayProgressBar("修改进度", count + "/" + Selection.gameObjects.Length + "完成修改", progress: count / Selection.gameObjects.Length);
        }

        EditorUtility.ClearProgressBar();
    }
}