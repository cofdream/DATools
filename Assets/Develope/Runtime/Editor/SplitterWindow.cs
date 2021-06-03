using UnityEngine;
using UnityEditor;

namespace DATools
{
    public class SplitterWindow : EditorWindow
    {
        [MenuItem("Test/Open")]
        static void Open()
        {
            GetWindow<SplitterWindow>().Show();
        }

        GUISplitData GUISplitData;
        GUISplitData GUISplitData2;
        GUISplitData GUISplitData3;
        GUISplitData GUISplitData4;

        private void OnEnable()
        {
            GUISplitData = new GUISplitData()
            {
                LinePos = LinePos.Right,
                X = 0,
                Y = 0,
                Width = 80f,
                Height = 100f,
                MaxWidth = 200f,
                MaxHeight = 200f,
            };

            GUISplitData2 = new GUISplitData()
            {
                LinePos = LinePos.Left,
                X = 300,
                Y = 0,
                Width = 80f,
                Height = 100f,
                MaxWidth = 200f,
                MaxHeight = 200f,
            };
        }

        private void OnGUI()
        {
            GUISplitView.BeginSplitView(ref GUISplitData);
            {
                GUILayout.Label("---左侧---左侧---左侧---");
                GUILayout.Label("---左侧---左侧---左侧---");
                GUILayout.Label("---左侧---左侧---左侧---");
                GUILayout.Label("---左侧---左侧---左侧---");
            }
            GUISplitView.EndSplitView(ref GUISplitData);

            GUISplitView.BeginSplitView(ref GUISplitData2);
            {
                GUILayout.Label("---右侧---右侧---右侧---");
                GUILayout.Label("---右侧---右侧---右侧---");
                GUILayout.Label("---右侧---右侧---右侧---");
                GUILayout.Label("---右侧---右侧---右侧---");
            }
            GUISplitView.EndSplitView(ref GUISplitData2);

            if (GUISplitData.Resize)
            {
                Repaint();
            }
        }
    }
}