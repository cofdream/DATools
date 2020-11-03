using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text;
using System.Text.RegularExpressions;

namespace DATools
{
    /// <summary>
    /// 创建文件工具
    /// </summary>
	public class CreateFileTool : EditorWindow
    {
        private const string DEFINE_CLASS_NAME = "New File";
        private const string DEFINE_SCRIPT_CLASS = "";

        [MenuItem("Assets/Create/File", false, 101)]
        private static void CreateFile()
        {
            string pathName = Utils.GetSelectionPath()[0] + "/" + DEFINE_CLASS_NAME;
            Texture2D icon = EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;

            var action = ScriptableObjectExpand.CreateInstanceOnly<CreateFileAction>();

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, pathName, icon, string.Empty);
        }

        private class CreateFileAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var nameWithoutExtension = Path.GetFileName(pathName);
                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                File.WriteAllText(pathName, string.Empty, encoding);

                AssetDatabase.ImportAsset(pathName);

                var obj = AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
                ProjectWindowUtil.ShowCreatedAsset(obj);
            }
        }
    }
}