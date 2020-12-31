﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace DATools
{
    internal static class DeleteNotRefresh
    {
        [MenuItem("Assets/Delete Not Refresh")]
        private static void Delete()
        {
            var paths = Utils.GetSelectionPath();

            string content = string.Empty;
            int length = paths.Length;
            for (int i = 0; i < length; i++)
            {
                if (i == 3)
                {
                    content += "...\n";
                    break;
                }

                content += paths[i] + "\n";
            }
            content += "You cannot undo this action.";

            if (!EditorUtility.DisplayDialog("Delete select assets?", content, "Delete", "Cancel"))
            {
                return;
            }

            //StringBuilder sb = new StringBuilder(paths.Length * 3 + 4);
            //sb.AppendLine("Delete Directorys:     Not Refresh Assets");
            //sb.AppendLine("↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
            foreach (var item in paths)
            {
                if (File.Exists(item))
                {
                    File.Delete(item);
                    //sb.AppendLine(item);
                }
                else
                {
                    if (Directory.Exists(item))
                    {
                        Directory.Delete(item);
                        //sb.AppendLine(item);
                    }
                }

                string meta = item + ".meta";
                if (File.Exists(meta))
                {
                    File.Delete(meta);
                    //sb.AppendLine(meta);
                    //sb.AppendLine();
                }
            }
            //sb.AppendLine("↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑");

            //Debug.LogWarning(sb.ToString());
        }

        [MenuItem("Assets/Delete Not Refresh", true)]
        private static bool IsDelete()
        {
            var paths = Utils.GetSelectionPath();
            if (paths.Length == 0 || paths.Length != Selection.objects.Length)
            {
                return false;
            }
            foreach (var item in paths)
            {
                if (!item.StartsWith("Assets/"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}