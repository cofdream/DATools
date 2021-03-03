using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace NullNamespace
{
    public class ChangeResolution : EditorWindow
    {
        [MenuItem("Test/Open")]
        private static void OpenChangeResolutionWindow()
        {
            GetWindow<ChangeResolution>(true).Show();
        }

        private object gameViewSizesInstance;
        private PropertyInfo currentGroup;

        private EditorWindow gameViewWindow;
        private MethodInfo sizeSelectionCallback;
        private int sizeSelectIndex = 0;

        private List<_GameViewSize> _GameViewSizes_Builtin;
        private List<_GameViewSize> _GameViewSizes_Custom;

        Vector2 scroolViewValue;
        private void Awake()
        {
            var type = Type.GetType("UnityEditor.GameViewSizes,UnityEditor");
            currentGroup = type.GetProperty("currentGroup", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            var singletonType = typeof(ScriptableSingleton<>).MakeGenericType(type);
            gameViewSizesInstance = singletonType.GetProperty("instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty).GetValue(null, null);


            Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
            gameViewWindow = EditorWindow.GetWindow(gameViewType);
            sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback", BindingFlags.Public | BindingFlags.Instance);


            object gameViewSizeGroup = currentGroup.GetValue(gameViewSizesInstance);
            Type gameViewSizeGroupType = gameViewSizeGroup.GetType();

            IEnumerable<object> m_builtion = gameViewSizeGroupType.GetField("m_Builtin", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameViewSizeGroup) as IEnumerable<object>;
            PropertyInfo displayText_Get = null;
            foreach (var item in m_builtion)
            {
                Type gameViewSizeType = item.GetType();
                displayText_Get = gameViewSizeType.GetProperty("displayText", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                break;
            }
            if (displayText_Get != null)
            {
                _GameViewSizes_Builtin = new List<_GameViewSize>();
                foreach (var gameViewSize in m_builtion)
                {
                    _GameViewSizes_Builtin.Add(new _GameViewSize()
                    {
                        displayText = displayText_Get.GetValue(gameViewSize).ToString(),
                    });
                }
            }


            IEnumerable<object> m_Custom = gameViewSizeGroupType.GetField("m_Custom", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameViewSizeGroup) as IEnumerable<object>;
            if (displayText_Get != null)
            {
                _GameViewSizes_Custom = new List<_GameViewSize>();
                foreach (var gameViewSize in m_Custom)
                {
                    _GameViewSizes_Custom.Add(new _GameViewSize()
                    {
                        displayText = displayText_Get.GetValue(gameViewSize).ToString(),
                    });
                }
            }
        }
        private void OnGUI()
        {
            if (_GameViewSizes_Builtin == null || _GameViewSizes_Custom == null)
                return;

            scroolViewValue = GUILayout.BeginScrollView(scroolViewValue);
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(EditorGUIUtility.FindTexture("RotateTool")))
                    {

                    }


                    int max = _GameViewSizes_Builtin.Count + _GameViewSizes_Custom.Count;
                    GUILayout.Label($"max: {max}");

                    if (int.TryParse(GUILayout.TextField(sizeSelectIndex.ToString(), GUILayout.Width(40)), out int index))
                    {
                        if (index <= max && index > 0)
                        {
                            sizeSelectIndex = index;
                        }
                    }

                    if (GUILayout.Button("Select"))
                    {
                        sizeSelectionCallback.Invoke(gameViewWindow, new object[] { sizeSelectIndex - 1, null });
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(2);

                foreach (var _GameViewSize in _GameViewSizes_Builtin)
                {

                    GUILayout.Label(_GameViewSize.displayText);
                }
                GUILayout.Label("");
                foreach (var _GameViewSize in _GameViewSizes_Custom)
                {
                    GUILayout.Label(_GameViewSize.displayText);
                }
            }
            GUILayout.EndScrollView();
        }
    }
    public class _GameViewSize
    {
        public string displayText;
    }
}