using UnityEngine;
using UnityEditor;

namespace DATools
{
    public class PackageVersionTool : EditorWindow
    {
        private void OnGUI()
        {
            // todo 获取git的版本

            // 用户选择对应版本去更新
        }


        [MenuItem("Tools/VersionUpdate")]
        private static void OpenPackageVersionTool()
        {
            var window = EditorMainWindow.GetWindowInCenter<PackageVersionTool>();
            window.Show();
        }
    }
}