using UnityEngine;
using UnityEditor;
using System;

namespace DATools
{
    public static class ScriptableObjectExpand
    {
        public static ScriptableObject CreateInstanceOnly(Type type)
        {
            var objs = Resources.FindObjectsOfTypeAll(type);
            if (objs != null && objs.Length != 0)
            {
                if (objs.Length > 1)
                    EditorUtility.DisplayDialog("Warring", $"Exist multiple instatnce!,\n type: {type}", "确定");
                return objs[0] as EditorWindow;
            }
            return ScriptableObject.CreateInstance(type);
        }
        public static T CreateInstanceOnly<T>() where T : ScriptableObject
        {
            return (T)CreateInstanceOnly(typeof(T));
        }
    }
}