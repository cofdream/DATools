using UnityEditor;
using UnityEngine;

namespace DATools
{
    public static class GUIExpand
    {
        // 绘制一个搜索框
        public static string DrawSearchField(Rect position, string text)
        {
            const float cancelButtonWidth = 14f;

            Rect buttonRect = position;
            buttonRect.x += position.width - cancelButtonWidth;
            buttonRect.width = cancelButtonWidth;

            //为按钮写点击事件
            if (Event.current.type == EventType.MouseUp && buttonRect.Contains(Event.current.mousePosition))
            {
                text = "";
                GUI.changed = true;
            }

            text = GUI.TextField(position, text, EditorStyles.toolbarSearchField);

            GUI.Button(buttonRect, GUIContent.none, !string.IsNullOrEmpty(text) ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");

            return text;
        }
    }
}