// ========================================================
// 描述：Chinar Inspector增强 —— 显示get/set属性
// 错误：如果遇到报错，请切换C# .Net版本
// 作者：Chinar 
// 创建时间：2018-12-30 15:22:19
// 版 本：1.0
// ========================================================
using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace ChinarX.Serializable
{
    [CustomPropertyDrawer(typeof(SerializableProperty))]
    public class ChinarSetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            var sp = (SerializableProperty) attribute;
            if (EditorGUI.EndChangeCheck())
            {
                if (sp != null) sp.IsChangeProperty = true;
            }
            else if (sp != null && sp.IsChangeProperty)
            {
                object       parent = GetParentObjectOfProperty(property.propertyPath, property.serializedObject.targetObject);
                PropertyInfo pi     = parent.GetType().GetProperty(sp.Name);
                if (pi == null)
                {
                    Debug.LogError("序列化属性失败: <" + sp.Name + ">\n请检查[SerializableProperty]属性");
                }
                else
                {
                    pi.SetValue(parent, fieldInfo.GetValue(parent), null);
                }

                sp.IsChangeProperty = false;
            }
        }


        private object GetParentObjectOfProperty(string path, object obj)
        {
            string[] strings = path.Split('.');
            if (strings.Length == 1) return obj;
            obj = obj.GetType().GetField(strings[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(obj);
            return GetParentObjectOfProperty(string.Join(".", strings, 1, strings.Length - 1), obj);
        }
    }
}