using UnityEngine;
using UnityEditor;

namespace DATools
{
    public struct GUISplitData
    {
        public Vector2 ViewPosition;

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float MinWidth;
        public float MinHeight;

        public float MaxWidth;
        public float MaxHeight;

        public bool IsSplitHorizontal;
        public bool IsSplitVertiacal;

        public bool ResizeHorizontal;
        public bool ResizeVertical;

        //public float LineSpace;
        //public float LineSpaceOffest;
        //public float lineWidth;

        public GUISplitData(float x, float y, float width, float height, float minWidth, float minHeight, float maxWidth, float maxHeight, bool isSplitHorizontal, bool isSplitVertiacal)
        {
            ViewPosition = Vector2.zero;

            X = x;
            Y = y;
            Width = width;
            Height = height;

            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;

            IsSplitHorizontal = isSplitHorizontal;
            IsSplitVertiacal = isSplitVertiacal;

            ResizeHorizontal = false;
            ResizeVertical = false;

            //LineSpace = 5;
            //LineSpaceOffest = 2;
            //lineWidth = 1;
        }
    }
    public static class GUISplitView
    {

        #region 整合ScroolView API

        public static void BeginSplitView(ref GUISplitData data, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
        }

        public static void BeginSplitView(ref GUISplitData data, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
        }

        public static void BeginSplitView(ref GUISplitData data, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal: false, alwaysShowVertical: false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
        }


        public static void BeginSplitView(ref GUISplitData data, GUIStyle style)
        {
            BeginSplitView(ref data, style, options: null);
        }

        public static void BeginSplitView(ref GUISplitData data, GUIStyle style, params GUILayoutOption[] options)
        {
            string name = style.name;
            GUIStyle gUIStyle = GUI.skin.FindStyle(name + "VerticalScrollbar");
            if (gUIStyle == null)
            {
                gUIStyle = GUI.skin.verticalScrollbar;
            }

            GUIStyle gUIStyle2 = GUI.skin.FindStyle(name + "HorizontalScrollbar");
            if (gUIStyle2 == null)
            {
                gUIStyle2 = GUI.skin.horizontalScrollbar;
            }

            BeginSplitView(ref data, alwaysShowHorizontal: false, alwaysShowVertical: false, gUIStyle2, gUIStyle, style, options);
        }

        public static void BeginSplitView(ref GUISplitData data, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
        }

        #endregion

        public static void BeginSplitView(ref GUISplitData data, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
        {
            GUILayoutOption[] newOptions;
            if (options != null)
            {
                newOptions = new GUILayoutOption[options.Length + 2];
                options.CopyTo(newOptions, 2);
            }
            else
                newOptions = new GUILayoutOption[2];


            newOptions[0] = GUILayout.Width(data.Width);
            newOptions[1] = GUILayout.Height(data.Height);

            data.ViewPosition = GUILayout.BeginScrollView(data.ViewPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, newOptions);
        }

        public static void EndSplitView(ref GUISplitData data)
        {
            GUILayout.EndScrollView();
            ResizeSplitView(ref data);
        }

        private static void ResizeSplitView(ref GUISplitData data)
        {
            float lineSpace = 5f;
            float lineSpaceOffest = 2f; //=lineSpace * 0.5 - 线段的宽度
            Color lineColor = new Color(0, 0, 0, 0.5f);


            //Rect resizeUpLeft = new Rect(data.X + data.Width - lineSpaceOffest, data.Y + data.Height - lineSpaceOffest, lineSpace, lineSpace);
            //EditorGUIUtility.AddCursorRect(resizeUpLeft, MouseCursor.ResizeUpLeft);

            //if (Event.current.type == EventType.MouseDown && resizeUpLeft.Contains(Event.current.mousePosition))
            //{
            //    data.ResizeVertical = true;
            //    data.ResizeHorizontal = true;
            //}

            //if (Event.current.type == EventType.MouseUp)
            //{
            //    data.ResizeVertical = false;
            //    data.ResizeHorizontal = false;
            //}


            if (data.IsSplitHorizontal)
            {
                Rect splitResizeLeftRightRect = new Rect(data.X + data.Width - lineSpaceOffest, data.Y, lineSpace, data.Height);
                EditorGUIUtility.AddCursorRect(splitResizeLeftRightRect, MouseCursor.SplitResizeLeftRight);

                //EditorGUI.DrawRect(splitResizeLeftRightRect, new Color(1, 1, 1, 0.1f));

                Rect lineRect = splitResizeLeftRightRect;
                lineRect.x += lineSpaceOffest;
                lineRect.width = 1f;
                EditorGUI.DrawRect(lineRect, lineColor);



                if (Event.current.type == EventType.MouseDown && splitResizeLeftRightRect.Contains(Event.current.mousePosition))
                    data.ResizeHorizontal = true;

                if (Event.current.type == EventType.MouseUp)
                    data.ResizeHorizontal = false;

                if (data.ResizeHorizontal && Event.current.type == EventType.MouseDrag)
                {
                    data.Width = Event.current.mousePosition.x;
                    if (data.Width > data.MaxWidth)
                    {
                        data.Width = data.MaxWidth;
                    }
                    else if (data.Width < data.MinWidth)
                    {
                        data.Width = data.MinWidth;
                    }
                }
            }

            if (data.IsSplitVertiacal)
            {
                Rect splitResizeUpDownRect = new Rect(data.X, data.Y + data.Height - lineSpaceOffest, data.Width, lineSpace);
                EditorGUIUtility.AddCursorRect(splitResizeUpDownRect, MouseCursor.ResizeVertical);

                //EditorGUI.DrawRect(splitResizeUpDownRect, new Color(1, 1, 1, 0.1f));

                Rect lineRect = splitResizeUpDownRect;
                lineRect.y += lineSpaceOffest;
                lineRect.height = 1f;
                EditorGUI.DrawRect(lineRect, lineColor);



                if (Event.current.type == EventType.MouseDown && splitResizeUpDownRect.Contains(Event.current.mousePosition))
                    data.ResizeVertical = true;

                if (Event.current.type == EventType.MouseUp)
                    data.ResizeVertical = false;


                if (data.ResizeVertical && Event.current.type == EventType.MouseDrag)
                {
                    EditorGUIUtility.AddCursorRect(splitResizeUpDownRect, MouseCursor.SplitResizeUpDown);

                    data.Height = Event.current.mousePosition.y;
                    if (data.Height > data.MaxHeight)
                    {
                        data.Height = data.MaxHeight;
                    }
                    else if (data.Height < data.MinHeight)
                    {
                        data.Height = data.MinHeight;
                    }
                }
            }

           
        }

        //todo Check GUISplitData 的范围，保证在窗口内
    }
}