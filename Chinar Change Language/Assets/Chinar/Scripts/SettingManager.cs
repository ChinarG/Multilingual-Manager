// ========================================================
// 描述：设置管理器
// 作者：Chinar 
// 创建时间：2018-12-28 14:30:35
// 版 本：1.0
// ========================================================
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Dropdown").GetComponent<Dropdown>().onValueChanged.AddListener(OnSelectLanguage);
        var texts  = Resources.LoadAll<GameObject>("DynamicText");
        var canvas = GameObject.Find("Canvas").transform;
        for (int i = 0; i < texts.Length; i++)
        {
            var s = Instantiate(texts[i], canvas);
        }
    }


    private void OnSelectLanguage(int value)
    {
        switch (value)
        {
            case 0:
                ChangeLanguage(value);
                break;
            case 1:
                ChangeLanguage(value);
                break;
            case 2:
                print("其他语言自行添加");
                break;
        }
    }


    /// <summary>
    /// 改变语言
    /// </summary>
    private void ChangeLanguage(int arg)
    {
        var texts = FindObjectsOfType<UiChinarText>();
        switch (arg)
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
        }
    }
}