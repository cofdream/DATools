using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace DATools
{
    public class PackageVersionTool : EditorWindow
    {
        private void OnGUI()
        {
            // todo 获取git的版本l

            // 用户选择对应版本去更新

            //if (GUILayout.Button("Test"))
            //{

            //    var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CmdOpenDirectory));
            //    newThread.Start();

            //}

        }

        //private static void CmdOpenDirectory(object obj)
        //{
        //    var p = new System.Diagnostics.Process();
        //    p.StartInfo.FileName = "cmd.exe";
        //    p.StartInfo.Arguments = @"E:\Git\DATools git tags";

        //    UnityEngine.Debug.Log(p.StartInfo.Arguments);

        //    p.StartInfo.UseShellExecute = false;
        //    p.StartInfo.RedirectStandardInput = true;
        //    p.StartInfo.RedirectStandardOutput = true;
        //    p.StartInfo.RedirectStandardError = true;
        //    p.StartInfo.CreateNoWindow = true;
        //    p.Start();

        //    p.WaitForExit();

        //    Debug.Log(p.StandardOutput.ReadToEnd());

        //    p.Close();
        //}


        [MenuItem("DATools/VersionUpdate")]
        private static void OpenPackageVersionTool()
        {
            var window = EditorMainWindow.GetWindowInCenter<PackageVersionTool>();
            window.Show();
        }
    }
}