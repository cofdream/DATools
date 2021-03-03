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
            var window = GetWindow<ChangeResolution>(true);
            window.maxSize = new Vector2(200, 500);
            window.Show();
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
        private int aa;
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

                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(this.maxSize.x), GUILayout.Height(15));
                foreach (var _GameViewSize in _GameViewSizes_Builtin)
                {
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        GUI.Label(rect, _GameViewSize.displayText, (GUIStyle)"LODSliderRangeSelected");
                        if (!GUI.changed)
                        {
                            GUI.changed = true;
                        }
                    }
                    else
                    {
                        GUI.Label(rect, _GameViewSize.displayText);
                    }
                    rect.position += new Vector2(0, 15);
                }

                GUI.Label(rect, "");

                foreach (var _GameViewSize in _GameViewSizes_Custom)
                {
                    rect.position += new Vector2(0, 15);

                    if (rect.Contains(Event.current.mousePosition))
                    {
                        GUI.Label(rect, _GameViewSize.displayText, (GUIStyle)"LODSliderRangeSelected");
                        if (!GUI.changed)
                        {
                            GUI.changed = true;
                        }
                    }
                    else
                    {
                        GUI.Label(rect, _GameViewSize.displayText);
                    }
                }
            }
            GUILayout.EndScrollView();

            if (GUI.changed)
            {
                base.Repaint();
                GUI.changed = false;
                Debug.Log(" Event.current.mousePosition" + Event.current.mousePosition);
            }
        }
    }
    public class _GameViewSize
    {
        public string displayText;
    }
}