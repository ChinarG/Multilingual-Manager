using System;
using UnityEditor;
using UnityEngine;
using static System.String;


public class ChinarWelcomeWindow : EditorWindow
{
    private          string              version           = "Version : 1.0.0";
    private readonly Rect                _versionRect      = new Rect(5f, 630f, 125f, 20f);
    private readonly Rect                _welcomeIntroRect = new Rect(120f, 15f, 666f, 40f);
    private static   ChinarWelcomeWindow windowChinarWelcom;
    private static   Item                chinarSitetexture = new Item(new Rect(66f, 66f, 266f, 266f), chinarSitetexture.Texture, null);
    private static   Item                weChatOr          = new Item(new Rect(332f, 66f, 266f, 266f), weChatOr.Texture, "点击加入 —— QQ群:806091680");
    private static   Item                qqGrouptexture    = new Item(new Rect(222f, 366f, 66f, 50f), qqGrouptexture.Texture, null);
    private          Item                qqGroupTitle      = new Item(new Rect(288f, 376f, 250f, 20f), null, " 加入技术支持社群");
    private          Item                qqGroupContent    = new Item(new Rect(288f, 396f, 250f, 30f), null, "点击加入 —— QQ群:806091680");


    private struct Item
    {
        public Rect    Rect;
        public Texture Texture;
        public string  Content;


        public Item(Rect rect, Texture texture, string content)
        {
            Rect    = rect;
            Texture = texture;
            Content = content;
        }
    }


    public void OnEnable()
    {
        chinarSitetexture.Texture = LoadTexture("ChinarSiteIcon.png");
        weChatOr.Texture          = LoadTexture("Wechat-Chinar.png");
        qqGrouptexture.Texture    = LoadTexture("QQ.png");
    }


    static Texture LoadTexture(string textureName)
    {
        return (Texture) AssetDatabase.LoadAssetAtPath("Assets/Chinar/Editor/Editor Default Resources/" + textureName, typeof(Texture));
    }


    [MenuItem("Chinar/欢迎界面", false, 1)]
    public static void ShowChinarWelcomeWindow()
    {
        windowChinarWelcom         = GetWindow<ChinarWelcomeWindow>(true, "关注 & 支持 Chinar");
        windowChinarWelcom.minSize = windowChinarWelcom.maxSize = new Vector2(666f, 666f);
        windowChinarWelcom.Show();
    }


    private void Link(Rect rect, Texture texture, string content, bool isLabel = true)
    {
        if (isLabel)
        {
            GUI.Label(rect, content);
        }
        else
        {
            GUI.DrawTexture(rect, texture);
        }

        EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
    }


    private void OnGUI()
    {
        GUI.Label(_welcomeIntroRect, "Chinar 的初衷是将一种简单快捷的生活方式带给世人,使有限时间，具备无限可能！");
        GUI.Label(new Rect(160f, 35f, 666f, 40f), "关注微信公众号 & 加入社区群 —— Chinar 将为您提供更优质内容");
        GUI.Label(_versionRect, version);
        GUI.DrawTexture(weChatOr.Rect, weChatOr.Texture);
        Link(qqGrouptexture.Rect, qqGrouptexture.Texture, null, false);
        Link(chinarSitetexture.Rect, chinarSitetexture.Texture, null, false);
        Link(qqGroupTitle.Rect, null, qqGroupTitle.Content);
        Link(qqGroupContent.Rect, null, qqGroupContent.Content);
        if (Event.current.type == EventType.MouseUp)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            if (qqGrouptexture.Rect.Contains(mousePosition) || qqGroupTitle.Rect.Contains(mousePosition) || qqGroupContent.Rect.Contains(mousePosition))
            {
                Application.OpenURL("http://shang.qq.com/wpa/qunwpa?idkey=5dacafe26abe29923ed6a5d8cf76248b5b68f0fcdc599fcd231007814eb75c4d");
            }
            else if (chinarSitetexture.Rect.Contains(mousePosition))
            {
                Application.OpenURL("http://www.chinar.xin");
            }
        }
    }
}


public class ChinarInitializeOnLoadWindow : EditorWindow
{
    private static ChinarInitializeOnLoadWindow chinarInitializeOnLoadWindow;
    private        float                        timeSpan = 0;
    private static bool                         isShow   = true;


    [InitializeOnLoadMethod]
    static void InitializeOnLoadWindow()
    {
        if (PlayerPrefs.GetString("Chinar_Improt_DateTime") == Empty)
        {
            PlayerPrefs.SetString("Chinar_Improt_DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        else
        {
            if ((DateTime.Now - DateTime.ParseExact(PlayerPrefs.GetString("Chinar_Improt_DateTime"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Days >= 7)
            {
                PlayerPrefs.SetString("Chinar_Improt_DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                isShow = false;
                return;
            }
        }

        chinarInitializeOnLoadWindow         = GetWindow<ChinarInitializeOnLoadWindow>(false, "Chinar InitPanel");
        chinarInitializeOnLoadWindow.minSize = chinarInitializeOnLoadWindow.maxSize = Vector2.zero;
    }


    private void Update()
    {
        timeSpan += Time.deltaTime;
        if (!(timeSpan > 0.5f) || !isShow) return;
        ChinarWelcomeWindow.ShowChinarWelcomeWindow();
        chinarInitializeOnLoadWindow.Close();
    }
}