using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEditor.PackageManager.UI;

namespace DATools
{
    public class ExportPackage : EditorWindow
    {
        // TODO 打算实现在编辑器就能吧对应路径资源包给打出去。目前懒得实现了。 还是用默认的

        //private string inputValue = string.Empty;
        //private string path = string.Empty;

        //[MenuItem("Tools/Export Package")]
        //private static void OpenExportPackage()
        //{
        //    var window = GetWindow<ExportPackage>("Export Package");
        //    window.position = EditorMainWindow.GetMainWindowCenteredPosition(new Vector2(850, 350));
        //    window.maximized = false;
        //    window.Show();
        //}

        private void OnGUI()
        {
            //EditorGUILayout.s("导出包保存的路径", (GUIStyle)"AM ToolbarObjectField");

            //GUILayout.BeginHorizontal();
            //{
            //    inputValue = EditorGUILayout.TextField(string.Empty, inputValue, (GUIStyle)"SearchTextField");

            //    if (GUILayout.Button(GUIContent.none, string.IsNullOrEmpty(inputValue) ? (GUIStyle)"SearchCancelButtonEmpty" : (GUIStyle)"SearchCancelButton"))
            //    {
            //        inputValue = string.Empty;
            //    }
            //    GUILayout.FlexibleSpace();
            //}
            //GUILayout.EndHorizontal();


            //if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            //{
            //    var tempPath = DragAndDrop.paths[0];

            //    if (tempPath.StartsWith("assets", StringComparison.OrdinalIgnoreCase))
            //    {
            //        tempPath = Application.dataPath + tempPath.Substring(6, tempPath.Length - 6);
            //    }

            //    path = tempPath;
            //}

            //GUILayout.BeginHorizontal();
            //{
            //    GUILayout.Label("包导出路径：");
            //    GUILayout.Space(8);
            //    path = GUILayout.TextField(path);

            //    if (GUILayout.Button("Browse..."))
            //    {
            //        Utils.OpenDirectory(path);
            //    }

            //    GUILayout.FlexibleSpace();
            //}
            //GUILayout.EndHorizontal();


            //GUILayout.BeginHorizontal();
            //{
            //    if (GUILayout.Button("导出包"))
            //    {
            //        EditorUtility.DisplayProgressBar("Export Package", "Wait Export!", 0);

            //        var fileName = string.Format("Repoitory.unitypackage", DateTime.Now.ToString("yyyyMMdd-HH-mm-ss"));

            //        AssetDatabase.ExportPackage(path, fileName, ExportPackageOptions.Recurse);

            //        //把导出的包移动到指定路径存放
            //        var basePath = Directory.GetParent(Application.dataPath).FullName;
            //        var fileSavePath = basePath + "/Export Packages";
            //        if (Directory.Exists(fileSavePath) == false)
            //        {
            //            Directory.CreateDirectory(fileSavePath);
            //        }

            //        var filePath = basePath + @"\" + fileName;

            //        if (File.Exists(filePath))
            //        {
            //            File.Move(filePath, fileSavePath + @"\" + fileName);
            //        }

            //        EditorUtility.DisplayProgressBar("Export Package", "Export The End!", 1);
            //        EditorUtility.ClearProgressBar();

            //        Application.OpenURL("file://" + fileSavePath);

            //        //EditorUtility.DisplayDialog("Export Packerage", "导出工具包已完成，路径已复制到粘贴板！", "确定");
            //        //GUIUtility.systemCopyBuffer = ;
            //    }
            //    GUILayout.FlexibleSpace();
            //}
            //GUILayout.EndHorizontal();
        }
    }

}