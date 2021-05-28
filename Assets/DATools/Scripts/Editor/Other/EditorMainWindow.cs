using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace DATools
{
    public static class EditorMainWindow
    {
        private static UnityEngine.Object mainWindow = null;

        public static T GetWindowInCenter<T>() where T : EditorWindow
        {
            return GetWindowInCenter<T>(new Vector2(600, 300));
        }
        public static T GetWindowInCenter<T>(Vector2 size) where T : EditorWindow
        {
            var window = EditorWindow.GetWindow<T>();
            window.position = GetMainWindowCenteredPosition(size);
            return window;
        }

        public static Rect GetMainWindowCenteredPosition(Vector2 size)
        {
            Rect parentWindowPosition = GetMainWindowPositon();
            var pos = new Rect
            {
                x = 0,
                y = 0,
                width = Mathf.Min(size.x, parentWindowPosition.width * 0.90f),
                height = Mathf.Min(size.y, parentWindowPosition.height * 0.90f)
            };
            var w = (parentWindowPosition.width - pos.width) * 0.5f;
            var h = (parentWindowPosition.height - pos.height) * 0.5f;
            pos.x = parentWindowPosition.x + w;
            pos.y = parentWindowPosition.y + h;
            return pos;
        }
        public static Rect GetMainWindowPositon()
        {
            if (mainWindow == null)
            {
                var containerWinType = AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).FirstOrDefault(t => t.Name == "ContainerWindow");
                if (containerWinType == null)
                    throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
                var showModeField = containerWinType.GetField("m_ShowMode", BindingFlags.NonPublic | BindingFlags.Instance);
                if (showModeField == null)
                    throw new MissingFieldException("Can't find internal fields 'm_ShowMode'. Maybe something has changed inside Unity");
                var windows = Resources.FindObjectsOfTypeAll(containerWinType);
                foreach (var win in windows)
                {
                    var showMode = (int)showModeField.GetValue(win);
                    if (showMode == 4)
                    {
                        mainWindow = win;
                        break;
                    }
                }
            }
            if (mainWindow == null)
            {
                Debug.LogError("Unity mainWindow API change,Please Check!");
                return new Rect(0f, 0f, 400f, 600f);
            }

            var positionProperty = mainWindow.GetType().GetProperty("position", BindingFlags.Public | BindingFlags.Instance);
            if (positionProperty == null)
                throw new MissingFieldException("Can't find internal fields 'position'. Maybe something has changed inside Unity.");

            return ((Rect)positionProperty.GetValue(mainWindow, null));
        }
        private static Type[] GetAllDerivedTypes(this AppDomain aAppDomain, Type aType)
        {
            return TypeCache.GetTypesDerivedFrom(aType).ToArray();
        }
    }
}