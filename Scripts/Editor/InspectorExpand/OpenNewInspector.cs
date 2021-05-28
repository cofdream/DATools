using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace DATools
{
    public class OpenNewInspector : EditorWindow
    {

        [MenuItem("DATools/Window/Inspector Dounle")]
        private static void Open()
        {
            var window = GetWindow<OpenNewInspector>();
            window.position = EditorWindowExtension.GetMainWindowCenteredPosition(new Vector2(500, 800));
            window.Show();
        }
        private List<MyInspectorInfo> selectGameObjectList = new List<MyInspectorInfo>() { new MyInspectorInfo(), new MyInspectorInfo(), new MyInspectorInfo() };
        private void OnGUI()
        {
            if (selectGameObjectList == null) selectGameObjectList = new List<MyInspectorInfo>() { new MyInspectorInfo(), new MyInspectorInfo(), new MyInspectorInfo() };

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < selectGameObjectList.Count; i++)
            {
                bool isChange = selectGameObjectList[i].SetValue(EditorGUILayout.ObjectField(selectGameObjectList[i].Target, typeof(GameObject), true) as GameObject);

                if (isChange)
                {
                    CreateNewInspectorWindow();
                    selectGameObjectList[i].Inspector = inspectorWindowInstance;
                }

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < selectGameObjectList.Count; i++)
            {
                if (selectGameObjectList[i].Inspector != null)
                {
                    inspectorWindowType.GetMethod("OnGUI", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(selectGameObjectList[i].Inspector, null);
                }
            }
            EditorGUILayout.EndHorizontal();
        }



        private static Type inspectorWindowType;
        private static PropertyInfo lockedProperyInfo;
        private static object isLocked;
        private static EditorWindow inspectorWindowInstance;
        private static UnityEngine.Object target;
        static OpenNewInspector()
        {
            inspectorWindowType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor");
            lockedProperyInfo = inspectorWindowType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
            isLocked = true;
        }

        [MenuItem("DATools/Window/Inspector For Selected")]
        private static void CreateNewInspectorWindow()
        {
            inspectorWindowInstance = ScriptableObject.CreateInstance(inspectorWindowType) as EditorWindow;

            lockedProperyInfo.SetValue(inspectorWindowInstance, isLocked);
        }

        [MenuItem("DATools/Window/Inspector For Selected( close last)")]
        public static void OpenNewInspectorWindow()
        {
            UnityEngine.Object target = Selection.activeObject;
            if (target == null) return;

            if (OpenNewInspector.target != null && OpenNewInspector.target.Equals(target) == false && inspectorWindowInstance != null)
            {
                inspectorWindowInstance.Close();
                UnityEngine.Object.DestroyImmediate(inspectorWindowInstance);
            }

            OpenNewInspector.target = target;
            CreateNewInspectorWindow();
        }


        private class MyInspectorInfo
        {
            public GameObject Target;
            public EditorWindow Inspector;

            public bool SetValue(GameObject target)
            {
                if (target == null)
                {
                    Inspector = null;
                    return false;
                }
                if (target != null && target.Equals(Target) == false)
                {
                    Target = target;
                    return true;
                }
                return false;
            }
        }

    }
}