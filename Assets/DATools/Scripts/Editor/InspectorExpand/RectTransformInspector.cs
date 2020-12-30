using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    [CustomEditor(typeof(RectTransform), true), CanEditMultipleObjects]
    public class RectTransformInspector : Editor
    {
        #region GUIContent
        private readonly GUIContent contentAnchoredPostion = new GUIContent(" AP ", (Texture)null, "当前物体的锚点坐标归0");
        private readonly GUIContent contentPosition = new GUIContent(" P ", (Texture)null, "当前物体的本地坐标归0");
        private readonly GUIContent contentRotation = new GUIContent(" R ", (Texture)null, "当前物体的本地旋转归0");
        private readonly GUIContent contentScale = new GUIContent(" S ", (Texture)null, "当前物体的本地缩放归1");
        private readonly GUIContent contentAddWindow = new GUIContent("增量修改"); 
        #endregion

        #region 增量面板
        private static Expand expand;

        private SerializedObject m_serializedObject;
        private SerializedProperty serializedPropertyPosition;
        private SerializedProperty serializedPropertyRotation;
        private SerializedProperty serializedPropertyScale;

        bool isOpenAddWindow;
        private bool isStartAdd;
        #endregion

        private Editor defaultEditor;

        private void OnEnable()
        {
            defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.RectTransformEditor, UnityEditor"));

            var objs = Resources.FindObjectsOfTypeAll<Expand>();
            if (objs.Length != 0)
            {
                expand = objs[0];
            }
            else
            {
                expand = ScriptableObject.CreateInstance<Expand>();
            }

            m_serializedObject = new SerializedObject(expand);
            serializedPropertyPosition = m_serializedObject.FindProperty("addPosition");
            serializedPropertyRotation = m_serializedObject.FindProperty("addRotation");
            serializedPropertyScale = m_serializedObject.FindProperty("addScale");
        }
        private void OnDisable()
        {
            if (defaultEditor != null)
                DestroyImmediate(defaultEditor);
            defaultEditor = null;

            expand = null;

            m_serializedObject = null;
            serializedPropertyPosition = null;
            serializedPropertyRotation = null;
            serializedPropertyScale = null;
        }
        private void OnDestroy()
        {
            if (defaultEditor != null)
                DestroyImmediate(defaultEditor);
            defaultEditor = null;

            expand = null;

            m_serializedObject = null;
            serializedPropertyPosition = null;
            serializedPropertyRotation = null;
            serializedPropertyScale = null;
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            DrawOnInspectorGUI();
        }

        private void DrawOnInspectorGUI()
        {
            bool isRestAnchoredPostion;
            bool isResetPosition;
            bool isResetRotation;
            bool isResetScale;

            GUILayout.BeginHorizontal();
            {
                isRestAnchoredPostion = GUILayout.Button(contentAnchoredPostion);
                isResetPosition = GUILayout.Button(contentPosition);
                isResetRotation = GUILayout.Button(contentRotation);
                isResetScale = GUILayout.Button(contentScale);

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            if (targets.Length > 1)
            {
                GUILayout.BeginHorizontal();
                {
                    isOpenAddWindow = GUILayout.Toggle(isOpenAddWindow, contentAddWindow, EditorStyles.foldoutHeader);

                    GUILayout.FlexibleSpace();

                    isStartAdd = GUILayout.Button("开始增量");
                }
                GUILayout.EndHorizontal();

                if (isOpenAddWindow)
                {
                    m_serializedObject.Update();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedPropertyPosition, new GUIContent("LocalPosition"));

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("重置"))
                    {
                        expand.addPosition = Vector3.zero;
                    }

                    GUILayout.EndHorizontal();

                    EditorGUI.BeginChangeCheck();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedPropertyRotation, new GUIContent("Rotation"));
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("重置"))
                    {
                        expand.addRotation = Vector3.zero;
                    }

                    GUILayout.EndHorizontal();


                    EditorGUI.BeginChangeCheck();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedPropertyScale, new GUIContent("Scale"));
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("重置"))
                    {
                        expand.addScale = Vector3.zero;
                    }

                    GUILayout.EndHorizontal();

                    m_serializedObject.ApplyModifiedProperties();


                }
            }




            foreach (var temp in targets)
            {
                var rectTransform = temp as RectTransform;

                if (isRestAnchoredPostion)
                {
                    Undo.RecordObject(rectTransform, "Reset anchoredPosition");
                    rectTransform.anchoredPosition = Vector3.zero;
                }

                if (isResetPosition)
                {
                    Undo.RecordObject(rectTransform, "Reset localPosition");
                    rectTransform.localPosition = Vector3.zero;
                }

                if (isResetRotation)
                {
                    Undo.RecordObject(rectTransform, "Reset localRotation");
                    rectTransform.localRotation = Quaternion.identity;

                    SerializedProperty m_LocalEulerAnglesHint = this.serializedObject.FindProperty("m_LocalEulerAnglesHint");
                    this.serializedObject.Update();
                    m_LocalEulerAnglesHint.vector3Value = Vector3.zero;
                    this.serializedObject.ApplyModifiedProperties();
                }

                if (isResetScale)
                {
                    Undo.RecordObject(rectTransform, "Reset localScale");
                    rectTransform.localScale = Vector3.one;
                }

                if (isStartAdd)
                {
                    Undo.RecordObject(rectTransform, "Add Change Transform Values");
                    rectTransform.localPosition += expand.addPosition;
                    rectTransform.localEulerAngles += expand.addRotation;
                    rectTransform.localScale += expand.addScale;
                }
            }

        }


        private class Expand : ScriptableObject
        {
            public Vector3 addPosition;
            public Vector3 addRotation;
            public Vector3 addScale;
        }
    }
}