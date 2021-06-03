using UnityEngine;
using UnityEditor;

namespace DATools
{

    public struct GUISplitData
    {
        public bool IsHorizontal;
        public Vector2 ViewPosition;
        public float Width;
        public float Height;
    }
    public static class GUISplitView
    {
        public static void BeginSplitView(ref GUISplitData data)
        {
            if (data.IsHorizontal)
                data.ViewPosition = GUILayout.BeginScrollView(data.ViewPosition, GUILayout.Width(data.Width), GUILayout.Width(data.Height));
            else
                data.ViewPosition = GUILayout.BeginScrollView(data.ViewPosition, GUILayout.Width(data.Width), GUILayout.Height(data.Height));
        }

        public static void EndSplitView(ref GUISplitData data)
        {
            GUILayout.EndScrollView();
            ResizeSplitView(ref data);
        }

        private static void ResizeSplitView(ref GUISplitData data)
        {
            Rect linePosition;
            if (data.IsHorizontal)
            {
                
            }
        }
    }
}