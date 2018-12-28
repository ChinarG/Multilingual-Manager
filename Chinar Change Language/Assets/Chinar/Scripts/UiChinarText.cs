// ========================================================
// 描述：
// 作者：Chinar 
// 创建时间：2018-12-28 14:21:48
// 版 本：1.0
// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


public class UiChinarText : MonoBehaviour
{
    public int    L;     //编号
    public string E;     //英
    public string C;     //中
    public Color  Color; //字体颜色


    public UiChinarText()
    {
    }


    public UiChinarText(int l, string e, string c, Color color, ref List<UiChinarTextData> datas)
    {
        L     = l;
        E     = e;
        C     = c;
        Color = color;
        UiChinarTextData data = new UiChinarTextData(l, e, c, color);
        datas.Add(data);
    }
}


public class Root
{
    [XmlArray("Items"), XmlArrayItem("item")] public List<UiChinarTextData> Datas;


    public Root()
    {
        Datas = new List<UiChinarTextData>();
    }
}

public class UiChinarTextData
{
    [XmlAttribute] public int    L; //编号
    [XmlAttribute] public string E; //英
    [XmlAttribute] public string C; //中
    public                Ys     ys;


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
    }


    public UiChinarTextData()
    {
    }


    public UiChinarTextData(int l, string e, string c, Color color)
    {
        L  = l;
        E  = e;
        C  = c;
        ys = new Ys(color.r, color.g, color.g, color.a);
    }
}