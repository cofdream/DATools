using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    [CustomEditor(typeof(Transform), true), CanEditMultipleObjects]
    internal sealed class TransformInspector : Editor
    {
        private readonly GUIContent contentPosition = new GUIContent(" P ", (Texture)null, "当前物体的本地坐标归0");
        private readonly GUIContent contentRotation = new GUIContent(" R ", (Texture)null, "当前物体的本地旋转归0");
        private readonly GUIContent contentScale = new GUIContent(" S ", (Texture)null, "当前物体的本地缩放归1");
        private readonly GUIContent contentAddWindow = new GUIContent("增量修改");


        #region 增量面板
        private static Expand expand;//自定义数据类，不使用基类的SerializedObject。

        private SerializedObject m_serializedObject;

        private SerializedProperty serializedPropertyPosition;
        private SerializedProperty serializedPropertyRotation;
        private SerializedProperty serializedPropertyScale;

        private bool isOpenAddWindow;
        private bool isStartAdd;
        #endregion

        private Editor defaultEditor;

        private static Type type = Type.GetType("UnityEditor.TransformInspector, UnityEditor");

        private void Awake()
        {
            if (defaultEditor == null)
            {
                defaultEditor = Editor.CreateEditor(targets, type);
            }
        }

        private void OnEnable()
        {
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

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            DrawOnInspectorGUI();
        }

        private void OnDestroy()
        {
            DestroyImmediate(defaultEditor);
        }

        private void DrawOnInspectorGUI()
        {
            bool isResetPosition;
            bool isResetRotation;
            bool isResetScale;

            GUILayout.BeginHorizontal();
            {
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
                    EditorGUILayout.PropertyField(serializedPropertyPosition, new GUIContent("Position"));

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
                var transform = temp as Transform;

                if (isResetPosition)
                {
                    Undo.RecordObject(transform, "Reset localPosition");
                    transform.localPosition = Vector3.zero;
                }

                if (isResetRotation)
                {

                    Undo.RecordObject(transform, "Reset localRotation");
                    transform.localRotation = Quaternion.identity;

                    SerializedProperty m_LocalEulerAnglesHint = this.serializedObject.FindProperty("m_LocalEulerAnglesHint");
                    this.serializedObject.Update();
                    m_LocalEulerAnglesHint.vector3Value = Vector3.zero;
                    this.serializedObject.ApplyModifiedProperties();
                }

                if (isResetScale)
                {
                    Undo.RecordObject(transform, "Reset localScale");
                    transform.localScale = Vector3.one;
                }

                if (isStartAdd)
                {
                    Undo.RecordObject(transform, "Add Change Transform Values");
                    transform.localPosition += expand.addPosition;
                    transform.localEulerAngles += expand.addRotation;
                    transform.localScale += expand.addScale;
                }
            }
        }

        private sealed class Expand : ScriptableObject
        {
            public Vector3 addPosition;
            public Vector3 addRotation;
            public Vector3 addScale;
        }
    }
}