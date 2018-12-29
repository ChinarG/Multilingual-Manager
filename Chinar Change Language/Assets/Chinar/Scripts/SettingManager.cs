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
    public LanguageData LanguageDatas;//语言包数据
    public bool         IsChangeLanguage;//是否加载过语言包数据，进行过更换就会加载一次数据，为了避免多次加载


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
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].ChangeLanguage(this, value);
        }

        IsChangeLanguage = true;
    }
}