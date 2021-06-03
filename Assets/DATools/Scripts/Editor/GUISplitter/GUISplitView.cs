using UnityEngine;
using UnityEditor;

namespace DATools
{
    public enum LinePos
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3,
    }
    public struct GUISplitData
    {
        public LinePos LinePos;
        public Vector2 ViewPosition;
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float MaxWidth;
        public float MaxHeight;

        public bool Resize;
    }
    public static class GUISplitView
    {
        public static void BeginSplitView(ref GUISplitData data)
        {
            data.Resize = false;
            data.ViewPosition = GUILayout.BeginScrollView(data.ViewPosition, GUILayout.Width(data.Width), GUILayout.Height(data.Height));
        }

        public static void EndSplitView(ref GUISplitData data)
        {
            GUILayout.EndScrollView();
            ResizeSplitView(ref data);
        }

        private static void ResizeSplitView(ref GUISplitData data)
        {
            Rect splitRect;

            float lineSpace = 3f;
            float lineSpaceOffest = 1f; //减少线段的宽度
            Color lineColor = new Color(0, 0, 0, 0.5f);

            switch (data.LinePos)
            {
                case LinePos.Left:
                    {
                        splitRect = new Rect(data.X - lineSpaceOffest, data.Y, lineSpace, data.Height);
                        EditorGUIUtility.AddCursorRect(splitRect, MouseCursor.ResizeHorizontal);

                        EditorGUI.DrawRect(splitRect, Color.white);


                        Rect lineRect = splitRect;
                        lineRect.x += lineSpaceOffest;
                        lineRect.width = 1f;
                        EditorGUI.DrawRect(lineRect, lineColor);
                        break;
                    }
                case LinePos.Right:
                    {
                        splitRect = new Rect(data.X + data.Width - lineSpaceOffest, data.Y, lineSpace, data.Height);
                        EditorGUIUtility.AddCursorRect(splitRect, MouseCursor.ResizeHorizontal);

                        EditorGUI.DrawRect(splitRect, Color.white);

                        Rect lineRect = splitRect;
                        lineRect.x += lineSpaceOffest;
                        lineRect.width = 1f;
                        EditorGUI.DrawRect(lineRect, lineColor);
                        break;
                    }
                case LinePos.Top:
                    {
                        splitRect = new Rect(data.X, data.Y - lineSpaceOffest, data.Width, lineSpace);
                        EditorGUIUtility.AddCursorRect(splitRect, MouseCursor.ResizeVertical);

                        EditorGUI.DrawRect(splitRect, Color.white);

                        Rect lineRect = splitRect;
                        lineRect.y += lineSpaceOffest;
                        lineRect.height = 1f;
                        EditorGUI.DrawRect(lineRect, lineColor);
                        break;
                    }
                case LinePos.Bottom:
                    {
                        splitRect = new Rect(data.X, data.Y + data.Height - lineSpaceOffest, data.Width, lineSpace);
                        EditorGUIUtility.AddCursorRect(splitRect, MouseCursor.ResizeVertical);

                        EditorGUI.DrawRect(splitRect, Color.white);

                        Rect lineRect = splitRect;
                        lineRect.y += lineSpaceOffest;
                        lineRect.height = 1f;
                        EditorGUI.DrawRect(lineRect, lineColor);
                        break;
                    }
                default:
                    return;
            }

            if (Event.current.type == EventType.MouseDown && splitRect.Contains(Event.current.mousePosition))
                isMove = true;

            if (Event.current.type == EventType.MouseUp)
                isMove = false;


            if (isMove && Event.current.type == EventType.MouseDrag)
            {
                if ((int)data.LinePos < 2)
                {
                    data.Width =  Event.current.mousePosition.x;
                    if (data.Width > data.MaxWidth)
                    {
                        data.Width = data.MaxWidth;
                    }
                    else if (data.Width < 0)
                    {
                        data.Width = 0;
                    }
                }
                else
                {
                    data.Height = Event.current.mousePosition.y;
                    if (data.Height > data.MaxHeight)
                    {
                        data.Height = data.MaxHeight;
                    }
                    else if (data.Height < 0)
                    {
                        data.Height = 0;
                    }
                }
                data.Resize = true;
            }
        }

        public static bool isMove;
    }
}