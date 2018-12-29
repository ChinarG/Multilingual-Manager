// ========================================================
// 描述：数据脚本 —— 每个Text上挂载一个，没有挂载的Text编辑器在生成数据时，会自动获取数据并记录
// 作者：Chinar 
// 创建时间：2018-12-28 14:21:48
// 版 本：1.0
// ========================================================
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class ChinarRecordText : MonoBehaviour
{
    private void Start()
    {
      
    }


    public int    L;     //编号
    public string E;     //英
    public string C;     //中
    public Color  Color; //字体颜色


    public ChinarRecordText()
    {
    }


    public ChinarRecordText(int l, string e, string c, Color color, ref List<ChinarRecordTextData> datas)
    {
        L     = l;
        E     = e;
        C     = c;
        Color = color;
        ChinarRecordTextData data = new ChinarRecordTextData(l, e, c, color);
        datas.Add(data);
    }
}

/// <summary>
/// 语言数据总类
/// </summary>
[XmlRoot("Root")]
public class LanguageData
{
    [XmlArray("Ds"), XmlArrayItem("D")]
    public List<ChinarRecordTextData> Datas;


    public LanguageData()
    {
        Datas = new List<ChinarRecordTextData>();
    }
}

/// <summary>
/// 语言数据模型
/// </summary>
public class ChinarRecordTextData
{
    [XmlAttribute] public int    L; //编号
    [XmlAttribute] public string E; //英
    [XmlAttribute] public string C; //中
    public                Ys     ys;//颜色


    public struct Ys
    {
        [XmlAttribute] public float r;
        [XmlAttribute] public float g;
        [XmlAttribute] public float b;
        [XmlAttribute] public float a;


        public Ys(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }


        public override string ToString()
        {
            return $"{nameof(r)}: {r}, {nameof(g)}: {g}, {nameof(b)}: {b}, {nameof(a)}: {a}";
        }
    }


    public ChinarRecordTextData()
    {
    }


    public ChinarRecordTextData(int l, string e, string c, Color color)
    {
        L  = l;
        E  = e;
        C  = c;
        ys = new Ys(color.r, color.g, color.b, color.a);
    }


    public override string ToString()
    {
        return $"{nameof(L)}: {L}, {nameof(E)}: {E}, {nameof(C)}: {C}, {nameof(ys)}: {ys}";
    }
}