﻿using System.Text;
using UnityEditor;

namespace DATools
{
    public static class Utils
    {
        /// <summary>
        /// 获取Project目录下选中对象的 文件夹 路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetSelectionFoldePath()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            for (int i = 0; i < guids.Length; i++)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                if (System.IO.Directory.Exists(path) == false)
                {
                    int index = path.LastIndexOf('/');
                    path = path.Substring(0, index);
                }
                guids[i] = path;
            }
            return guids;
        }
        /// <summary>
        /// 获取Project目录下选中对象的路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetSelectionPath()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            for (int i = 0; i < guids.Length; i++)
            {
                guids[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return guids;
        }


        public static void OpenDirectory(string path, bool useCMD = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                UnityEngine.Debug.Log($"{path} 是Null or WhiteSpace");
            }
            else
            {
                path = path.Replace("/", "\\");//反转 防止找不到文件
                if (System.IO.Directory.Exists(path))
                {
                    path = $"{path}";//路径存在空格会打开错误,加双引号可以解决这个问题
                    if (useCMD)
                        OpenDirectoryByCMD(path);
                    else
                        OpenDirectoryByEXPER(path); //Window10 无法打开不清楚问题
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"{path} 不是文件夹夹路径");
                }
            }
        }
        private static void OpenDirectoryByEXPER(string path)
        {
            System.Diagnostics.Process.Start("exper.exe", path);
        }
        private static void OpenDirectoryByCMD(string path)
        {
            // 新开线程防止锁死
            var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CmdOpenDirectory));
            newThread.Start(path);
        }
        private static void CmdOpenDirectory(object obj)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c start " + obj.ToString();
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            process.WaitForExit();

            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());

            process.Close();
        }

        public class CmdInfo
        {
            public string CMDContent;
            public string Title;
            public string Info;
        }
        public static string Cmd(CmdInfo[] cmdInfos)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");


            StringBuilder output = new StringBuilder("cmd:\n");

            int length = cmdInfos.Length;
            float progress;
            for (int i = 0; i < length; i++)
            {
                process.Start();

                var cmdInfo = cmdInfos[i];

                progress = (float)i / length;
                EditorUtility.DisplayProgressBar(cmdInfo.Title, cmdInfo.Info, progress);

                process.StandardInput.WriteLine(cmdInfo.CMDContent);
                process.StandardInput.AutoFlush = true;

                output.AppendLine(cmdInfo.CMDContent);
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();

                output.AppendLine();
                output.AppendLine(process.StandardOutput.ReadToEnd());
            }

            process.Close();

            EditorUtility.ClearProgressBar();


            return output.ToString();
        }
    }
}