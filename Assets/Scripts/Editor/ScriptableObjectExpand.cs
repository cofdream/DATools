using UnityEngine;
using UnityEditor;
using System;

public static class ScriptableObjectExpand
{
    public static ScriptableObject CreateInstanceOnly(Type type)
    {
        var objs = Resources.FindObjectsOfTypeAll(type);
        if (objs != null)
        {
            if (objs.Length != 0)
            {
                if (objs.Length > 1)
                    Debug.LogWarning("Exist multiple instatnce!");
                return objs[0] as EditorWindow;
            }
        }
        return ScriptableObject.CreateInstance(type);
    }
    public static T CreateInstanceOnly<T>() where T : ScriptableObject
    {
        return (T)CreateInstanceOnly(typeof(T));
    }
}