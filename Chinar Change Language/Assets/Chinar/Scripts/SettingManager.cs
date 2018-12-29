// ========================================================
// 描述：设置管理器
// 作者：Chinar 
// 创建时间：2018-12-28 14:30:35
// 版 本：1.0
// ========================================================
using ChinarX.Tool;
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    public static SettingManager instance; //单例

    public static SettingManager Instance
    {
        get
        {
            if (instance == null) instance = new SettingManager();
            return instance;
        }
    }
    public LanguageData LanguageDatas;


    private void Awake()
    {
        LanguageDatas = Chinar.LoadXml<LanguageData>(Application.streamingAssetsPath + "/" + "Language.chinar");
    }


    void Start()
    {
        GameObject.Find("Dropdown").GetComponent<Dropdown>().onValueChanged.AddListener(OnSelectLanguage); //监听下拉菜单
        //加载动态加载文本资源
        var texts  = Resources.LoadAll<GameObject>("DynamicText");
        var canvas = GameObject.Find("Canvas").transform;
        for (int i = 0; i < texts.Length; i++)
        {
            var s = Instantiate(texts[i], canvas);
        }
    }


    /// <summary>
    /// 下拉菜单改变语言
    /// </summary>
    private void OnSelectLanguage(int value)
    {
        var texts = FindObjectsOfType<ChinarRecordText>();
        switch (value)
        {
            case 0:
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].GetComponent<Text>().text = texts[i].C;
                }

                break;
            case 1:
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].GetComponent<Text>().text = texts[i].E;
                }

                break;
            case 2:
                print("其他语言自行添加");
                break;
        }
    }
}