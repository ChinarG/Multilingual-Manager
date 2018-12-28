// ========================================================
// 描述：
// 作者：Chinar 
// 创建时间：2018-12-28 17:29:41
// 版 本：1.0
// ========================================================
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;



namespace ChinarX.Tool
{
    public class Chinar
    {
        /// <summary>
        /// 创建XML
        /// </summary>
        /// <param name="fileName">XML文件路径：包含文件名，扩展名</param>
        /// <param name="t">泛型数据</param>
        /// <param name="isEncryption">是否加密</param>
        public static void CreateXml<T>(string fileName, T t, bool isEncryption = false)
        {
            StreamWriter  writer        = File.CreateText(fileName);
            MemoryStream  memoryStream  = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            new XmlSerializer(typeof(T)).Serialize(xmlTextWriter, t);
            memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
            if (isEncryption)
            {
                RijndaelManaged rDel = new RijndaelManaged
                {
                    Key     = Encoding.UTF8.GetBytes("12348578902223367877723456789012"),
                    Mode    = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                UTF8Encoding encoding = new UTF8Encoding();
                writer.Write(Convert.ToBase64String(rDel.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())), 0, Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())).Length), 0, rDel.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())), 0, Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())).Length).Length));
            }
            else
            {
                writer.Write(new UTF8Encoding().GetString(memoryStream.ToArray()));
            }

            writer.Close();
        }


        /// <summary>
        /// 读取Xml
        /// </summary>
        /// <param name="path">XML文件路径：包含文件名，扩展名</param>
        /// <param name="isEncryption">是否加密</param>
        /// <returns>数据类型</returns>
        public static T LoadXml<T>(string path, bool isEncryption = false)
        {
            StreamReader sReader    = File.OpenText(path);
            string       dataString = sReader.ReadToEnd();
            sReader.Close();
            if (!isEncryption) return (T) new XmlSerializer(typeof(T)).Deserialize(new MemoryStream(new UTF8Encoding().GetBytes(dataString)));
            RijndaelManaged rDel = new RijndaelManaged
            {
                Key     = Encoding.UTF8.GetBytes("12348578902223367877723456789012"),
                Mode    = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            return (T) new XmlSerializer(typeof(T)).Deserialize(new MemoryStream(new UTF8Encoding().GetBytes(Encoding.UTF8.GetString(rDel.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(dataString), 0, Convert.FromBase64String(dataString).Length)))));
        }
    }
}