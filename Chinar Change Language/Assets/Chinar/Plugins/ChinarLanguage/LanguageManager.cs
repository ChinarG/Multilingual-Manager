// ========================================================
// 描述：设置管理器
// 作者：Chinar 
// 创建时间：2018-12-28 14:30:35
// 版 本：1.0
// ========================================================


using System;
using ChinarX.Serializable;
using UnityEngine;
using UnityEngine.UI;



namespace ChinarX.Plugin.Language
{
    /// <summary>
    /// 语种类型
    /// </summary>
    public enum LanguageType
    {
        Chinese, //中文
        English, //英文
        Other    //其他语言自行扩增
    }


    /// <summary>
    /// 设置管理器 
    /// </summary>
    public class LanguageManager : MonoBehaviour
    {
        public event Action LanguageEvent;    //
        public LanguageData LanguageDatas;    //语言包数据
        public bool         IsChangeLanguage; //是否加载过语言包数据，进行过更换就会加载一次数据，为了避免多次加载
        [SerializeField, SerializableProperty("DefaultLanguageType")]
        private LanguageType defaultLanguageType = LanguageType.Chinese; //默认中文
        public LanguageType DefaultLanguageType
        {
            get => defaultLanguageType;
            set
            {
                defaultLanguageType = value;
                LanguageEvent?.Invoke();
            }
        }


        private void Awake()
        {
            LanguageDatas =  Chinar.LoadXml<LanguageData>(Application.streamingAssetsPath + "/" + "Language.chinar");
            LanguageEvent += OnLanguageTypeChange;
        }


        /// <summary>
        /// 监听属性变动回调函数
        /// </summary>
        private void OnLanguageTypeChange()
        {
            var texts = FindObjectsOfType<ChinarRecordText>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].ChangeLanguage(this, DefaultLanguageType);
            }

            IsChangeLanguage = true;
        }


        void Start()
        {
            GameObject.Find("Dropdown").GetComponent<Dropdown>().onValueChanged.AddListener(OnSelectLanguage); //监听下拉菜单
            //动态加载文本资源
            var texts  = Resources.LoadAll<GameObject>("DynamicText");
            var canvas = GameObject.Find("Canvas").transform;
            for (int i = 0; i < texts.Length; i++)
            {
                var s = Instantiate(texts[i], canvas);
            }
        }


        /// <summary>
        /// 下拉菜单回调函数
        /// </summary>
        private void OnSelectLanguage(int value)
        {
            DefaultLanguageType = (LanguageType) value;
        }
    }
}