using UnityEngine;
using UnityEditor;

namespace DATools
{
    public class ReplaceClipboardString : EditorWindow, IDevelopementTool
    {
        public string ToolName => "替换粘体板字符串";

        [SearchTools("替换粘体板字符串")]
        [MenuItem("DATools/Replace Clipboard String Window")]
        static void OpenWindow()
        {
            EditorWindowExtension.GetWindowInCenter<ReplaceClipboardString>().Show();
        }


        public void OnGUI()
        {
            if (GUILayout.Button("替换粘贴板反斜杠为斜杠"))
            {
                GUIUtility.systemCopyBuffer = ReplaceBackslash(GUIUtility.systemCopyBuffer);
            }
        }
        public string ReplaceBackslash(string content)
        {
            return content.Replace('\\', '/');
        }

        public void Awake()
        {
        }

        public void OnDestroy()
        {
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
        }

    }
}